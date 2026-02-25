using Argus.WMS.MasterData.Reels;
using Volo.Abp.Application.Dtos;

namespace Argus.WMS.Putaway.Dtos
{
    public class GetPutawayTaskListInput : PagedAndSortedResultRequestDto
    {
        public PutawayTaskStatus? Status { get; set; }
    }
}