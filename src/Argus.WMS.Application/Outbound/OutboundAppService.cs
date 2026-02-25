using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.Application.Mappers;
using Argus.WMS.Inventorys;
using Argus.WMS.MasterData.Reels;
using Argus.WMS.Outbound.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.Outbound
{
    public class OutboundAppService :
        CrudAppService<OutboundOrder, OutboundOrderDto, Guid, PagedAndSortedResultRequestDto, CreateOutboundOrderInput>,
        IOutboundAppService
    {
        private readonly IRepository<OutboundOrder, Guid> _orderRepository;
        private readonly IRepository<PickTask, Guid> _pickTaskRepository;
        private readonly IRepository<Inventory, Guid> _inventoryRepository;
        private readonly IRepository<Reel, Guid> _reelRepository;
        private readonly AllocationManager _allocationDomainService;
        private readonly OutboundOrderApplicationMappers _orderMapper;
        private readonly PickTaskApplicationMappers _pickTaskMapper;
        private readonly OutboundOrderManager _outboundOrderManager;
    
        public OutboundAppService(
            IRepository<OutboundOrder, Guid> orderRepository,
            IRepository<PickTask, Guid> pickTaskRepository,
            IRepository<Inventory, Guid> inventoryRepository,
            IRepository<Reel, Guid> reelRepository,
            AllocationManager allocationDomainService,
            OutboundOrderApplicationMappers orderMapper,
            PickTaskApplicationMappers pickTaskMapper,
            OutboundOrderManager outboundOrderManager)
            : base(orderRepository)
        {
            _orderRepository = orderRepository;
            _pickTaskRepository = pickTaskRepository;
            _inventoryRepository = inventoryRepository;
            _reelRepository = reelRepository;
            _allocationDomainService = allocationDomainService;
            _orderMapper = orderMapper;
            _pickTaskMapper = pickTaskMapper;
            _outboundOrderManager = outboundOrderManager;
        }

        public override async Task<OutboundOrderDto> CreateAsync(CreateOutboundOrderInput input)
        {
            var itemTuples = input.Items
                .Select(x => (x.ProductCode, x.TargetLength, x.Quantity))
                .ToList();

            var order = await _outboundOrderManager.CreateOrderAsync(
                input.SourceOrderNo,
                input.CustomerName,
                itemTuples);

            return _orderMapper.Map(order);
        }

        public override async Task<PagedResultDto<OutboundOrderDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return await base.GetListAsync(input);
        }

        public async Task<List<PickTaskDto>> GeneratePickTaskAsync(Guid orderId)
        {
            var orderQuery = await _orderRepository.WithDetailsAsync(x => x.Items);
            var order = await AsyncExecuter.FirstOrDefaultAsync(orderQuery.Where(x => x.Id == orderId));

            if (order is null)
            {
                throw new UserFriendlyException("出库单不存在");
            }

            if (order.Status != OutboundOrderStatus.Created && order.Status != OutboundOrderStatus.PartiallyAllocated)
            {
                throw new UserFriendlyException("该出库单当前状态不允许执行分配操作。");
            }

            var tasks = new List<PickTaskDto>();
            var allItemsFullyAllocated = true;

            foreach (var item in order.Items)
            {
                var needToAllocateQuantity = item.Quantity - item.AllocatedQuantity;
                if (needToAllocateQuantity <= 0)
                {
                    continue;
                }

                try
                {
                    var allocationResults = await _allocationDomainService.AllocateAsync(
                        item.ProductCode,
                        item.TargetLength,
                        needToAllocateQuantity);

                    foreach (var result in allocationResults)
                    {
                        var reelQuery = await _reelRepository.WithDetailsAsync(x => x.CurrentLocation);
                        var reel = await AsyncExecuter.FirstOrDefaultAsync(reelQuery.Where(x => x.Id == result.ReelId));

                        if (reel is null)
                        {
                            throw new UserFriendlyException($"未找到盘具：{result.ReelId}");
                        }

                        reel.Lock("Outbound");
                        await _reelRepository.UpdateAsync(reel);

                        var task = new PickTask(
                            GuidGenerator.Create(),
                            order.Id,
                            item.Id,
                            result.InventoryId,
                            reel.ReelNo,
                            reel.CurrentLocation?.Code,
                            null);

                        await _pickTaskRepository.InsertAsync(task);
                        tasks.Add(_pickTaskMapper.Map(task));

                        item.IncreaseAllocatedQuantity(1);
                    }
                }
                catch (UserFriendlyException)
                {
                    allItemsFullyAllocated = false;
                }
            }

            if (tasks.Count == 0)
            {
                throw new UserFriendlyException("库存不足，未能分配任何可用盘具。");
            }

            if (allItemsFullyAllocated)
            {
                order.MarkAllocated();
            }
            else
            {
                order.MarkPartiallyAllocated();
            }

            await _orderRepository.UpdateAsync(order);

            return tasks;
        }

        /// <inheritdoc />
        public async Task CompletePickTaskAsync(Guid taskId)
        {
            var task = await _pickTaskRepository.FindAsync(taskId);
            if (task is null)
            {
                throw new UserFriendlyException("拣货任务不存在");
            }

            if (task.Status == PickTaskStatus.Completed)
            {
                return;
            }

            task.MarkCompleted();
            await _pickTaskRepository.UpdateAsync(task);

            var orderQuery = await _orderRepository.WithDetailsAsync(x => x.Items);
            var order = await AsyncExecuter.FirstOrDefaultAsync(orderQuery.Where(x => x.Id == task.OutboundOrderId));
            if (order is null)
            {
                throw new UserFriendlyException("出库单不存在");
            }

            var orderItem = order.Items.FirstOrDefault(x => x.Id == task.OutboundOrderItemId);
            if (orderItem is null)
            {
                throw new UserFriendlyException("出库单明细不存在");
            }

            var inventory = await _inventoryRepository.FindAsync(task.InventoryId);
            if (inventory is null)
            {
                throw new UserFriendlyException("库存记录不存在");
            }

            inventory.DeductLockedQuantity(orderItem.TargetLength);

            var reelId = inventory.ReelId;
            var inventoryQuery = await _inventoryRepository.GetQueryableAsync();
            var relatedInventoryIds = await AsyncExecuter.ToListAsync(
                inventoryQuery.Where(x => x.ReelId == reelId).Select(x => x.Id));

            if (inventory.Quantity <= 0)
            {
                await _inventoryRepository.DeleteAsync(inventory);
            }
            else
            {
                await _inventoryRepository.UpdateAsync(inventory);
            }

            var pickTaskQuery = await _pickTaskRepository.GetQueryableAsync();
            var remainingTaskCount = await AsyncExecuter.CountAsync(
                pickTaskQuery.Where(x => x.Status != PickTaskStatus.Completed && relatedInventoryIds.Contains(x.InventoryId)));

            if (remainingTaskCount == 0)
            {
                var reel = await _reelRepository.FindAsync(reelId);
                if (reel is not null)
                {
                    reel.UnLock();
                    await _reelRepository.UpdateAsync(reel);
                }
            }

            var orderTaskQuery = await _pickTaskRepository.GetQueryableAsync();
            var hasIncompleteTasks = await AsyncExecuter.AnyAsync(
                orderTaskQuery.Where(x => x.OutboundOrderId == task.OutboundOrderId && x.Status != PickTaskStatus.Completed));

            if (!hasIncompleteTasks)
            {
                order.MarkCompleted();
                await _orderRepository.UpdateAsync(order);
            }
        }

        protected override async Task<OutboundOrder> GetEntityByIdAsync(Guid id)
        {
            var query = await Repository.WithDetailsAsync(x => x.Items);
            return query.FirstOrDefault(x => x.Id == id);
        }

        protected override async Task<IQueryable<OutboundOrder>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
        {
            return await Repository.WithDetailsAsync(x => x.Items);
        }

        protected override OutboundOrderDto MapToGetOutputDto(OutboundOrder entity)
        {
            return _orderMapper.Map(entity);
        }

        protected override OutboundOrderDto MapToGetListOutputDto(OutboundOrder entity)
        {
            return _orderMapper.Map(entity);
        }
    }
}
