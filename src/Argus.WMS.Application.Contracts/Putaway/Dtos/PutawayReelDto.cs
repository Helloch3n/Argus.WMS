using System;
using System.Collections.Generic;
using Argus.WMS.MasterData.Reels;
using Volo.Abp.Application.Dtos;

namespace Argus.WMS.Putaway.Dtos
{
    public class PutawayReelDto : EntityDto<Guid>
    {
        public string ReelNo { get; set; }
        public string LocationCode { get; set; }
        public ReelStatus ReelStatus { get; set; }
        public bool IsLocked { get; set; }

        public bool IsMixed { get; set; }
        public string DisplayProductName { get; set; }
        public string DisplayQuantity { get; set; }

        public List<PutawayReelItemDto> Items { get; set; } = new();
    }
}