using System;
using Argus.WMS.MasterData.Warehouses;
using Argus.WMS.MasterData.Warehouses.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Argus.WMS.Application.Mappers;

[Mapper]
public partial class WarehouseApplicationMappers : MapperBase<Warehouse, WarehouseDto>
{
    public override partial WarehouseDto Map(Warehouse source);
    public override partial void Map(Warehouse source, WarehouseDto destination);
}