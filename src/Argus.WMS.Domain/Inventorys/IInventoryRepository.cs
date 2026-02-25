using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.Inventorys
{
    public interface IInventoryRepository : IRepository<Inventory, Guid>
    {
        Task<int> GetMaxLayerOnReelAsync(Guid reelId);
        Task<List<Inventory>> GetAllocatableInventoriesAsync(string productCode, decimal targetLength);
        Task<bool> IsSnExistsAsync(string sn);
    }
}