using Argus.WMS.Putaway;
using Argus.WMS.Putaway.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Argus.WMS.Application.Mappers;

[Mapper]
public partial class PutawayTaskApplicationMappers : MapperBase<PutawayTask, PutawayTaskDto>
{
    public override partial PutawayTaskDto Map(PutawayTask source);
    public override partial void Map(PutawayTask source, PutawayTaskDto destination);
}