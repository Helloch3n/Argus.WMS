using System;
using Argus.WMS.MasterData.Locations;
using Argus.WMS.MasterData.Locations.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Argus.WMS.Application.Mappers;

[Mapper]
public partial class LocationApplicationMappers : MapperBase<Location, LocationDto>
{
    public override partial LocationDto Map(Location source);
    public override partial void Map(Location source, LocationDto destination);
}