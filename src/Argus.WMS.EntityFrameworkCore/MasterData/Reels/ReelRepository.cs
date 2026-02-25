using Argus.WMS.EntityFrameworkCore;
using Argus.WMS.MasterData.Warehouses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Argus.WMS.MasterData.Reels
{
    public class ReelRepository
        : EfCoreRepository<WMSDbContext, Reel, Guid>,
        IReelRepository
    {
        public ReelRepository(IDbContextProvider<WMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<Reel>> GetAvailableForPutawayListAsync(
            string filter,
            Guid? warehouseId,
            string sorting,
            int skipCount,
            int maxResultCount)
        {
            var queryable = await GetQueryableAsync();

            var query = queryable
                .Include(x => x.CurrentLocation)
                .Include(x => x.Inventorys)
                    .ThenInclude(i => i.Product)
                .Where(x => x.Status == ReelStatus.Occupied)
                .Where(x => !x.IsLocked)
                .Where(x => x.CurrentLocation.Type == LocationType.Dock);

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(x => x.ReelNo.Contains(filter));
            }

            if (warehouseId.HasValue)
            {
                query = query.Where(x => x.CurrentLocation.WarehouseId == warehouseId.Value);
            }

            var sortingValue = string.IsNullOrWhiteSpace(sorting)
                ? nameof(Reel.ReelNo)
                : sorting;

            return await query
                .OrderBy(sortingValue)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<long> GetAvailableForPutawayCountAsync(string filter, Guid? warehouseId)
        {
            var queryable = await GetQueryableAsync();

            var query = queryable
                .Where(x => x.Status == ReelStatus.Occupied)
                .Where(x => !x.IsLocked)
                .Where(x => x.CurrentLocation.Type == LocationType.Dock);

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(x => x.ReelNo.Contains(filter));
            }

            if (warehouseId.HasValue)
            {
                query = query.Where(x => x.CurrentLocation.WarehouseId == warehouseId.Value);
            }

            return await query.LongCountAsync();
        }

        public async Task<Reel?> GetByReelNoWithLocationAsync(string reelNo)
        {
            var queryable = await GetQueryableAsync();
            return await queryable
                .Include(x => x.CurrentLocation)
                .FirstOrDefaultAsync(x => x.ReelNo == reelNo);
        }
    }
}