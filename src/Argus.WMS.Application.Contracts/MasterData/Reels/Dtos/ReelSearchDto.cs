using Volo.Abp.Application.Dtos;

namespace Argus.WMS.MasterData.Reels.Dtos
{
    public class ReelSearchDto : PagedAndSortedResultRequestDto
    {
        public string? ReelCode { get; set; }
    }
}
