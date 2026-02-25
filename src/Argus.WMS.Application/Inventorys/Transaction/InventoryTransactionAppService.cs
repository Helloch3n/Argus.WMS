using Argus.WMS.Application.Contracts.Inventorys.Transaction.Dtos;
using Argus.WMS.Application.Mappers;
using Argus.WMS.Inventorys;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.Application.Inventorys.Transaction
{
    public class InventoryTransactionAppService : ApplicationService, IApplicationService
    {
        private readonly IRepository<InventoryTransaction, System.Guid> _transactionRepository;
        private readonly InventoryTransactionMapper _transactionMapper;

        public InventoryTransactionAppService(
            IRepository<InventoryTransaction, System.Guid> transactionRepository,
            InventoryTransactionMapper transactionMapper)
        {
            _transactionRepository = transactionRepository;
            _transactionMapper = transactionMapper;
        }

        public async Task<PagedResultDto<InventoryTransactionDto>> GetListAsync(InventoryTransactionSearchDto input)
        {
            var query = await _transactionRepository.WithDetailsAsync(
                x => x.Reel,
                x => x.Product,
                x => x.FromLocation,
                x => x.ToLocation,
                x => x.FromWarehouse,
                x => x.ToWarehouse);

            query = query
                .WhereIf(!string.IsNullOrWhiteSpace(input.BillNo),
                    x => x.BillNo.Contains(input.BillNo!))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ReelNo),
                    x => x.Reel.ReelNo.Contains(input.ReelNo!))
                .WhereIf(input.ProductId.HasValue,
                    x => x.ProductId == input.ProductId!.Value)
                .WhereIf(input.Type.HasValue,
                    x => x.Type == input.Type!.Value)
                .WhereIf(input.StartTime.HasValue,
                    x => x.CreationTime >= input.StartTime!.Value)
                .WhereIf(input.EndTime.HasValue,
                    x => x.CreationTime <= input.EndTime!.Value);

            var totalCount = await AsyncExecuter.CountAsync(query);

            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
                query = query.OrderBy(input.Sorting);
            }
            else
            {
                query = query.OrderByDescending(x => x.CreationTime);
            }

            query = query.PageBy(input.SkipCount, input.MaxResultCount);
            var items = await AsyncExecuter.ToListAsync(query);

            var dtos = items.Select(_transactionMapper.Map).ToList();

            return new PagedResultDto<InventoryTransactionDto>(totalCount, dtos);
        }
    }
}