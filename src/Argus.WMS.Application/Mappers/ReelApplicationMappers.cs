using Argus.WMS.MasterData.Reels;
using Argus.WMS.MasterData.Reels.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Argus.WMS.Mappers;

[Mapper]
public partial class ReelApplicationMappers : MapperBase<Reel, ReelDto>
{
    [MapProperty(nameof(Reel.IsLocked), nameof(ReelDto.IsLocked))]
    public override partial ReelDto Map(Reel source);

    public override partial void Map(Reel source, ReelDto destination);

    public partial void Map(CreateUpdateReelDto source, Reel destination);
}