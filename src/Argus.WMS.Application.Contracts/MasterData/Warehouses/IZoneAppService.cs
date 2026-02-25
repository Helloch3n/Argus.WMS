using System;
using System.Threading.Tasks;
using Argus.WMS.MasterData.Warehouses.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Argus.WMS.MasterData.Warehouses
{
    public interface IZoneAppService : IApplicationService
    {
        Task<ZoneDto> GetAsync(Guid id);
        Task<ListResultDto<ZoneDto>> GetListAsync();
        Task<ZoneDto> CreateAsync(CreateUpdateZoneDto input);
        Task<ZoneDto> UpdateAsync(Guid id, CreateUpdateZoneDto input);
        Task DeleteAsync(Guid id);
    }
}
