using Argus.WMS.EntityFrameworkCore;
using Argus.WMS.MasterData.Locations;
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
            var dbContext = await GetDbContextAsync();

            var query =
                from reel in queryable
                join location in dbContext.Locations on reel.CurrentLocationId equals location.Id
                where reel.Status == ReelStatus.Occupied
                where !reel.IsLocked
                where location.Type == LocationType.Dock
                select new { reel, location };

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(x => x.reel.ReelNo.Contains(filter));
            }

            if (warehouseId.HasValue)
            {
                query = query.Where(x => x.location.WarehouseId == warehouseId.Value);
            }

            var sortingValue = string.IsNullOrWhiteSpace(sorting)
                ? nameof(Reel.ReelNo)
                : sorting;

            return await query
                .OrderBy($"reel.{sortingValue}")
                .Skip(skipCount)
                .Take(maxResultCount)
                .Select(x => x.reel)
                .ToListAsync();
        }

        public async Task<long> GetAvailableForPutawayCountAsync(string filter, Guid? warehouseId)
        {
            var queryable = await GetQueryableAsync();
            var dbContext = await GetDbContextAsync();

            var query =
                from reel in queryable
                join location in dbContext.Locations on reel.CurrentLocationId equals location.Id
                where reel.Status == ReelStatus.Occupied
                where !reel.IsLocked
                where location.Type == LocationType.Dock
                select new { reel, location };

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(x => x.reel.ReelNo.Contains(filter));
            }

            if (warehouseId.HasValue)
            {
                query = query.Where(x => x.location.WarehouseId == warehouseId.Value);
            }

            return await query.LongCountAsync();
        }

        public async Task<Reel?> GetByReelNoWithLocationAsync(string reelNo)
        {
            var queryable = await GetQueryableAsync();
            return await queryable.FirstOrDefaultAsync(x => x.ReelNo == reelNo);
        }
    }
}