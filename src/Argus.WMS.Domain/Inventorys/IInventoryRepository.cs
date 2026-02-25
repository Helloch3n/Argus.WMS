using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.Inventorys
{
    public interface IInventoryRepository : IRepository<Inventory, Guid>
    {
        Task<int> GetMaxLayerOnReelAsync(Guid reelId);
    }
}