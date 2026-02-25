using Argus.WMS.MasterData;
using Argus.WMS.MasterData.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Argus.WMS.Mappers;

[Mapper]
public partial class ProductApplicationMappers : MapperBase<Product, ProductDto>
{
    public override partial ProductDto Map(Product source);
    public override partial void Map(Product source, ProductDto destination);
}