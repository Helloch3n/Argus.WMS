using System;
using System.Threading.Tasks;
using Argus.WMS.MasterData.Warehouses.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Argus.WMS.MasterData.Warehouses
{
    public interface IWarehouseAppService : IApplicationService
    {
        Task<WarehouseDto> GetAsync(Guid id);
        Task<WarehouseWithDetailsDto> GetWithDetailsAsync(Guid id);
        Task<PagedResultDto<WarehouseDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<WarehouseDto> CreateAsync(CreateUpdateWarehouseDto input);
        Task<WarehouseDto> UpdateAsync(Guid id, CreateUpdateWarehouseDto input);
        Task DeleteAsync(Guid id);
    }
}
