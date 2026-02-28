using System;
using System.ComponentModel.DataAnnotations;

namespace Argus.WMS.MasterData.Reels.Dtos
{
    public class CreateUpdateReelDto
    {
        public string ReelNo { get; set; }

        [Required]
        public string Name { get; set; }

        public string Size { get; set; }
        public decimal SelfWeight { get; set; }
        public Guid? CurrentLocationId { get; set; }
    }
}