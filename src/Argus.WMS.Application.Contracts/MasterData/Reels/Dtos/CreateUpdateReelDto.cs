using System;
using System.ComponentModel.DataAnnotations;
using Argus.WMS.MasterData.Reels;

namespace Argus.WMS.MasterData.Reels.Dtos
{
    public class CreateUpdateReelDto
    {
        public string ReelNo { get; set; }

        [Required]
        public string Name { get; set; }

        public ReelType Type { get; set; }
        public decimal SelfWeight { get; set; }
        public decimal MaxWeight { get; set; }
        public Guid? CurrentLocationId { get; set; }
    }
}