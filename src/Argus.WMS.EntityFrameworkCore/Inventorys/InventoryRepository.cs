using System;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.EntityFrameworkCore;
using Argus.WMS.Inventorys;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Argus.WMS.EntityFrameworkCore.Inventorys
{
    public class InventoryRepository : EfCoreRepository<WMSDbContext, Inventory, Guid>, IInventoryRepository
    {
        public InventoryRepository(IDbContextProvider<WMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<int> GetMaxLayerOnReelAsync(Guid reelId)
        {
            return await (await GetDbSetAsync())
                .Where(x => x.ReelId == reelId)
                .MaxAsync(x => (int?)x.Index) ?? 0;
        }
    }
}