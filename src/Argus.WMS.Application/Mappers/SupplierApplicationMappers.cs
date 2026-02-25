using Argus.WMS.MasterData;
using Argus.WMS.MasterData.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Argus.WMS.Mappers;

[Mapper]
public partial class SupplierApplicationMappers : MapperBase<Supplier, SupplierDto>
{
    public override partial SupplierDto Map(Supplier source);
    public override partial void Map(Supplier source, SupplierDto destination);
}