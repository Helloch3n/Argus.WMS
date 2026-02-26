using System;
using Argus.WMS.MasterData.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Argus.WMS.MasterData
{
    public interface IProductAppService : ICrudAppService<ProductDto, Guid, ProductSearchDto, CreateUpdateProductDto>
    {
    }
}