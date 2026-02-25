using System;
using Argus.WMS.MasterData.Reels;
using Volo.Abp.Application.Dtos;

namespace Argus.WMS.Putaway.Dtos
{
    public class PutawayTaskDto : EntityDto<Guid>
    {
        public string TaskNo { get; set; }
        public string ReelNo { get; set; }
        public string FromLocationCode { get; set; }
        public string ToLocationCode { get; set; }
        public PutawayTaskStatus Status { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public ReelStatus ReelStatus { get; set; }
        public string ReelMaterialName { get; set; }
    }
}