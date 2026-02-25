using System.Threading.Tasks;
using Argus.WMS.Application.Contracts.Inventorys.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Argus.WMS.Application.Contracts.Inventorys
{
    public interface IInventoryAppService : IApplicationService
    {
        Task<ListResultDto<InventoryDto>> GetListAsync(InventorySearchDto input);
        //Task<InventoryDto> ProductionReceiveAsync(ProductionReceiveInput input);
    }
}
