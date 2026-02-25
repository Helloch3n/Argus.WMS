using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.MasterData.Reels
{
    public interface IReelRepository : IRepository<Reel, Guid>
    {
        Task<List<Reel>> GetAvailableForPutawayListAsync(
            string filter,
            Guid? warehouseId,
            string sorting,
            int skipCount,
            int maxResultCount);

        Task<long> GetAvailableForPutawayCountAsync(string filter, Guid? warehouseId);

        Task<Reel?> GetByReelNoWithLocationAsync(string reelNo);
    }
}