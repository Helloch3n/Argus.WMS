using System;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Argus.WMS.Inbound
{
    public class ReceiptRepository
        : EfCoreRepository<WMSDbContext, Receipt, Guid>,
          IReceiptRepository
    {
        public ReceiptRepository(IDbContextProvider<WMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Receipt?> GetWithDetailsAsync(Guid id)
        {
            var queryable = await GetQueryableAsync();

            return await queryable
                .Include(x => x.Details)
                    .ThenInclude(x => x.Reel)
                        .ThenInclude(r => r.CurrentLocation)
                .Include(x => x.Details)
                    .ThenInclude(x => x.Reel)
                        .ThenInclude(r => r.Inventorys)
                .Include(x => x.Details)
                    .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}