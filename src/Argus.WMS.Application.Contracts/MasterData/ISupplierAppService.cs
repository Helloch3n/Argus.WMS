using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Argus.WMS.MasterData.Dtos;
using System.Threading.Tasks;

namespace Argus.WMS.MasterData
{
    public interface ISupplierAppService :
        ICrudAppService<
            SupplierDto,
            Guid,
            SupplierSearchDto,
            CreateUpdateSupplierDto>
    {
        Task<PagedResultDto<SupplierDto>> GetListAsync(SupplierSearchDto input);
    }
}