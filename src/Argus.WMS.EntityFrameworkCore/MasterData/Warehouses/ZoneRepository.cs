using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.EntityFrameworkCore;
using Argus.WMS.MasterData.Zones;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Argus.WMS.MasterData.Warehouses
{
    public class ZoneRepository : EfCoreRepository<WMSDbContext, Zone, Guid>, IZoneRepository
    {
        public ZoneRepository(IDbContextProvider<WMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Zone?> GetByCodeAsync(string code)
        {
            var queryable = await GetQueryableAsync();
            return await queryable.FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<List<Zone>> GetListByWarehouseIdAsync(Guid warehouseId)
        {
            var queryable = await GetQueryableAsync();
            return await queryable.Where(x => x.WarehouseId == warehouseId).ToListAsync();
        }
    }
}
