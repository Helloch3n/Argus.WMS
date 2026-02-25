using System;
using System.Threading.Tasks;
using Argus.WMS.MasterData.Reels.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Argus.WMS.MasterData.Reels
{
    public interface IReelAppService :
        ICrudAppService<ReelDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateReelDto>
    {
        Task UpdateLocationAsync(Guid id, Guid newLocationId);
    }
}