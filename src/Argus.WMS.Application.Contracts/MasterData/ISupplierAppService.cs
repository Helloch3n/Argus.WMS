using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Argus.WMS.MasterData.Dtos;

namespace Argus.WMS.MasterData
{
    public interface ISupplierAppService :
        ICrudAppService<
            SupplierDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateSupplierDto>
    {
    }
}