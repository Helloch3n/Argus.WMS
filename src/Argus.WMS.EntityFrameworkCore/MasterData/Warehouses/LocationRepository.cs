using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.EntityFrameworkCore;
using Argus.WMS.MasterData.Locations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Argus.WMS.MasterData.Warehouses
{
    public class LocationRepository : EfCoreRepository<WMSDbContext, Location, Guid>, ILocationRepository
    {
        public LocationRepository(IDbContextProvider<WMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Location?> GetByCodeAsync(string code)
        {
            var queryable = await GetQueryableAsync();
            return await queryable.FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<List<Location>> GetListByZoneIdAsync(Guid zoneId)
        {
            var queryable = await GetQueryableAsync();
            return await queryable.Where(x => x.ZoneId == zoneId).ToListAsync();
        }

        public async Task<List<Location>> GetListByWarehouseIdAsync(Guid warehouseId)
        {
            var queryable = await GetQueryableAsync();
            return await queryable.Where(x => x.WarehouseId == warehouseId).ToListAsync();
        }
    }
}

