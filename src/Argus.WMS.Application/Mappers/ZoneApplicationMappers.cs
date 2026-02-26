using System;
using Argus.WMS.MasterData.Zones;
using Argus.WMS.MasterData.Zones.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Argus.WMS.Application.Mappers;

[Mapper]
public partial class ZoneApplicationMappers : MapperBase<Zone, ZoneDto>
{
    public override partial ZoneDto Map(Zone source);
    public override partial void Map(Zone source, ZoneDto destination);
}