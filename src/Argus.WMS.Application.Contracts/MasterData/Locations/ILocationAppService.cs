using System;
using System.Threading.Tasks;
using Argus.WMS.MasterData.Locations.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Argus.WMS.MasterData.Locations
{
    public interface ILocationAppService : IApplicationService
    {
        Task<LocationDto> GetAsync(Guid id);
        Task<PagedResultDto<LocationDto>> GetListAsync(LocationSearchDto input);
        Task<LocationDto> CreateAsync(CreateUpdateLocationDto input);
        Task<LocationDto> UpdateAsync(Guid id, CreateUpdateLocationDto input);
        Task DeleteAsync(Guid id);
        Task<ListResultDto<LocationDto>> BatchCreateAsync(BatchCreateLocationDto input);
        Task EnsureExistsAsync(Guid id);
    }
}
