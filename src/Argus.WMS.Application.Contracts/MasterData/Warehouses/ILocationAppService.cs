using System;
using System.Threading.Tasks;
using Argus.WMS.MasterData.Warehouses.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Argus.WMS.MasterData.Warehouses
{
    public interface ILocationAppService : IApplicationService
    {
        Task<LocationDto> GetAsync(Guid id);
        Task<ListResultDto<LocationDto>> GetListAsync();
        Task<LocationDto> CreateAsync(CreateUpdateLocationDto input);
        Task<LocationDto> UpdateAsync(Guid id, CreateUpdateLocationDto input);
        Task DeleteAsync(Guid id);
        Task BatchCreateAsync(BatchCreateLocationDto input);
    }
}
