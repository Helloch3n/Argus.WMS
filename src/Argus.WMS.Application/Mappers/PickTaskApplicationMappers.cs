using Argus.WMS.Outbound;
using Argus.WMS.Outbound.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Argus.WMS.Application.Mappers
{
    [Mapper]
    public partial class PickTaskApplicationMappers : MapperBase<PickTask, PickTaskDto>
    {
        public override partial PickTaskDto Map(PickTask source);

        public override partial void Map(PickTask source, PickTaskDto destination);
    }
}