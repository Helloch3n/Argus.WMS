using Argus.WMS.Application.Contracts.Inbound.Dtos;
using Argus.WMS.Application.Mappers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Argus.WMS.Inbound
{
    public class ReceiptAppService : ApplicationService, IReceiptAppService
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly ReceiptManager _receiptManager;
        private readonly ReceiptApplicationMappers _receiptMapper;

        public ReceiptAppService(
            IReceiptRepository receiptRepository,
            ReceiptManager receiptManager,
            ReceiptApplicationMappers receiptMapper)
        {
            _receiptRepository = receiptRepository;
            _receiptManager = receiptManager;
            _receiptMapper = receiptMapper;
        }

        public async Task<ReceiptDto> GetAsync(Guid id)
        {
            var receipt = await _receiptRepository.GetWithDetailsAsync(id)
                ?? throw new UserFriendlyException("收货单不存在");
            return _receiptMapper.Map(receipt);
        }

        public async Task<PagedResultDto<ReceiptDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _receiptRepository.GetQueryableAsync();

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            var items = await AsyncExecuter.ToListAsync(
                queryable
                    .OrderByDescending(x => x.CreationTime)
                    .PageBy(input.SkipCount, input.MaxResultCount));

            var dtos = items.Select(_receiptMapper.Map).ToList();

            return new PagedResultDto<ReceiptDto>(totalCount, dtos);
        }

        public async Task<ReceiptDto> CreateAsync(ReceiptCreateDto input)
        {
            var receipt = new Receipt(
                GuidGenerator.Create(),
                input.Type,
                ReceiptStatus.Draft,
                input.WarehouseId,
                input.BillNo);

            foreach (var detail in input.Details)
            {
                receipt.AddDetail(new ReceiptDetail(
                    GuidGenerator.Create(),
                    receipt.Id,
                    detail.ReelId,
                    detail.ProductId,
                    detail.PlanQuantity,
                    detail.Unit,
                    detail.SN,
                    detail.BatchNo,
                    detail.Source_WO,
                    detail.ToWarehouseId,
                    detail.CraftVersion,
                    detail.Layer_Index,
                    detail.Status));
            }

            await _receiptManager.CreateAsync(receipt);

            var entity = await _receiptRepository.GetWithDetailsAsync(receipt.Id);

            return _receiptMapper.Map(entity);
        }

        public async Task ExecuteAsync(ExecuteReceiptInput input)
        {
            var args = ObjectMapper.Map<ExecuteReceiptInput, ReceiveInventoryArgs>(input);
            await _receiptManager.ExecuteReceiptAsync(args);
        }

        public async Task ExecuteByReelAsync(ExecuteReceiptByReelInput input)
        {
            var receipt = await _receiptRepository.GetWithDetailsAsync(input.ReceiptId)
                ?? throw new UserFriendlyException("收货单不存在");

            var pendingDetails = receipt.Details
                .Where(x => x.ReelId == input.ReelId && !x.IsReceived)
                .ToList();

            if (pendingDetails.Count == 0)
            {
                throw new UserFriendlyException("该载具下没有待收货明细，或已全部收货");
            }
            foreach (var detail in pendingDetails)
            {
                var detailArgs = new ReceiveInventoryArgs(
                    detail.ReceiptId,
                    detail.Id,
                    detail.ActualQuantity,
                    detail.Weight,
                    input.LocationId,
                    detail.SN,
                    detail.Weight);
                await _receiptManager.ExecuteReceiptAsync(detailArgs);
            }
        }
    }
}