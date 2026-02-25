using System;
using System.ComponentModel.DataAnnotations;

namespace Argus.WMS.Application.Contracts.Inventorys.Dtos
{
    public class ProductionReceiveInput
    {
        public Guid ReelId { get; set; }
        public Guid ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Weight { get; set; }
        public string BatchNo { get; set; }
        public string Source_WO { get; set; }
        public Guid LocationId { get; set; }
        public string Unit { get; set; }
        [Required]
        [StringLength(100)]
        public string SN { get; set; }

        [StringLength(50)]
        public string? CraftVersion { get; set; }
    }
}