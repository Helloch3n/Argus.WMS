using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.Inventorys;
using Argus.WMS.Outbound.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.Outbound
{
    /// <summary>
    /// �������Ӧ�÷���
    /// </summary>
    public class PickTaskAppService : ApplicationService, IPickTaskAppService
    {
        private readonly IRepository<PickTask, Guid> _pickTaskRepository;
        private readonly IRepository<Inventory, Guid> _inventoryRepository;
        private readonly IRepository<OutboundOrder, Guid> _outboundOrderRepository;

        public PickTaskAppService(
            IRepository<PickTask, Guid> pickTaskRepository,
            IRepository<Inventory, Guid> inventoryRepository,
            IRepository<OutboundOrder, Guid> outboundOrderRepository)
        {
            _pickTaskRepository = pickTaskRepository;
            _inventoryRepository = inventoryRepository;
            _outboundOrderRepository = outboundOrderRepository;
        }

        /// <inheritdoc />
        public async Task<PagedResultDto<PickTaskDto>> GetListAsync(GetPickTaskListInput input)
        {
            var pickTaskQuery = await _pickTaskRepository.GetQueryableAsync();
            var inventoryQuery = await _inventoryRepository.WithDetailsAsync(x => x.Product);
            var orderQuery = await _outboundOrderRepository.WithDetailsAsync(x => x.Items);
            var orderItemQuery = orderQuery.SelectMany(x => x.Items);

            var query = from task in pickTaskQuery
                join inventory in inventoryQuery on task.InventoryId equals inventory.Id
                join item in orderItemQuery on task.OutboundOrderItemId equals item.Id
                select new { task, inventory, item };

            query = query
                .WhereIf(!string.IsNullOrWhiteSpace(input.ReelNo), x => x.task.ReelNo.Contains(input.ReelNo!))
                .WhereIf(input.Status.HasValue, x => x.task.Status == input.Status!.Value);

            var totalCount = await AsyncExecuter.CountAsync(query);

            var pageQuery = query
                .OrderByDescending(x => x.task.CreationTime)
                .PageBy(input.SkipCount, input.MaxResultCount);

            var items = await AsyncExecuter.ToListAsync(pageQuery);
            var dtoList = items.Select(x => new PickTaskDto
            {
                Id = x.task.Id,
                OutboundOrderId = x.task.OutboundOrderId,
                OutboundOrderItemId = x.task.OutboundOrderItemId,
                InventoryId = x.task.InventoryId,
                ReelNo = x.task.ReelNo,
                FromLocationCode = x.task.FromLocation,
                ToLocationCode = x.task.ToLocation,
                Status = x.task.Status,
                CreationTime = x.task.CreationTime,
                ProductCode = x.inventory.Product?.Code ?? x.item.ProductCode,
                TargetLength = x.item.TargetLength,
                BatchNo = x.inventory.BatchNo,
                Sn = x.inventory.SN
            }).ToList();

            return new PagedResultDto<PickTaskDto>(totalCount, dtoList);
        }

        /// <inheritdoc />
        public async Task<List<PickTaskDto>> GetListByOutboundOrderIdAsync(Guid outboundOrderId)
        {
            var pickTaskQuery = await _pickTaskRepository.GetQueryableAsync();
            var inventoryQuery = await _inventoryRepository.WithDetailsAsync(x => x.Product);
            var orderQuery = await _outboundOrderRepository.WithDetailsAsync(x => x.Items);
            var orderItemQuery = orderQuery.SelectMany(x => x.Items);

            var query = from task in pickTaskQuery
                join inventory in inventoryQuery on task.InventoryId equals inventory.Id
                join item in orderItemQuery on task.OutboundOrderItemId equals item.Id
                where task.OutboundOrderId == outboundOrderId
                select new { task, inventory, item };

            var items = await AsyncExecuter.ToListAsync(query.OrderByDescending(x => x.task.CreationTime));

            return items.Select(x => new PickTaskDto
            {
                Id = x.task.Id,
                OutboundOrderId = x.task.OutboundOrderId,
                OutboundOrderItemId = x.task.OutboundOrderItemId,
                InventoryId = x.task.InventoryId,
                ReelNo = x.task.ReelNo,
                FromLocationCode = x.task.FromLocation,
                ToLocationCode = x.task.ToLocation,
                Status = x.task.Status,
                CreationTime = x.task.CreationTime,
                ProductCode = x.inventory.Product?.Code ?? x.item.ProductCode,
                TargetLength = x.item.TargetLength,
                BatchNo = x.inventory.BatchNo,
                Sn = x.inventory.SN
            }).ToList();
        }
    }
}