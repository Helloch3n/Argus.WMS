using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.MasterData.Zones
{
    public interface IZoneRepository : IRepository<Zone, Guid>
    {
        Task<Zone?> GetByCodeAsync(string code);
        Task<List<Zone>> GetListByWarehouseIdAsync(Guid warehouseId);
    }
}
