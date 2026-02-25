using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.EntityFrameworkCore;
using Argus.WMS.Inventorys;
using Argus.WMS.MasterData.Reels;
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

        public async Task<List<Inventory>> GetAllocatableInventoriesAsync(string productCode, decimal targetLength)
        {
            var query = await GetQueryableAsync();
            var candidates = await query
                .Include(x => x.Reel)
                .Include(x => x.Product)
                .Where(x => x.Product.Code == productCode)
                .Where(x => x.Reel.Status == ReelStatus.Occupied)
                .Where(x => !x.Reel.IsLocked)
                .ToListAsync();

            return candidates
                .GroupBy(x => x.ReelId)
                .Select(g => g.OrderByDescending(x => x.Index).First())
                .Where(x => x.AvailableQuantity >= targetLength)
                .OrderBy(x => x.CreationTime)
                .ToList();
        }

        public async Task<bool> IsSnExistsAsync(string sn)
        {
            return await (await GetDbSetAsync()).AnyAsync(x => x.SN == sn);
        }
    }
}