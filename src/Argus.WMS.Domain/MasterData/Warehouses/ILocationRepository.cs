using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.MasterData.Warehouses
{
    public interface ILocationRepository : IRepository<Location, Guid>
    {
        Task<Location?> GetByCodeAsync(string code);
        Task<List<Location>> GetListByZoneIdAsync(Guid zoneId);
        Task<List<Location>> GetListByWarehouseIdAsync(Guid warehouseId);
    }
}

