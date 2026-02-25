using System.ComponentModel.DataAnnotations;

namespace Argus.WMS.MasterData.Dtos
{
    public class CreateUpdateProductDto
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public string Unit { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Length { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Width { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Height { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Weight { get; set; }

        public bool IsBatchManagementEnabled { get; set; }

        public int? ShelfLifeDays { get; set; }
    }
}