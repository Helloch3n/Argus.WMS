using Argus.WMS.DataSync;
using Argus.WMS.DataSync.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Argus.WMS.Mappers;

[Mapper]
public partial class DataSyncTaskApplicationMappers : MapperBase<DataSyncTask, DataSyncTaskDto>
{
    public override partial DataSyncTaskDto Map(DataSyncTask source);
    public override partial void Map(DataSyncTask source, DataSyncTaskDto destination);
}
