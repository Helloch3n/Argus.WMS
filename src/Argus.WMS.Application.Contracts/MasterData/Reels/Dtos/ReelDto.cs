using System;
using Volo.Abp.Application.Dtos;
using Argus.WMS.MasterData.Reels;

namespace Argus.WMS.MasterData.Reels.Dtos
{
    public class ReelDto : FullAuditedEntityDto<Guid>
    {
        public string ReelNo { get; set; }
        public string Name { get; set; }
        public ReelType Type { get; set; }
        public bool IsLocked { get; set; }
        public decimal SelfWeight { get; set; }
        public decimal MaxWeight { get; set; }
        public ReelStatus Status { get; set; }
        public Guid? CurrentLocationId { get; set; }
        public string CurrentLocationCode { get; set; }
    }
}