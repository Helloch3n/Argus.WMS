using System;
using Argus.WMS.Inventorys;

namespace Argus.WMS.Application.Contracts.Inventorys.Dtos
{
    public class InventoryDto
    {
        public Guid Id { get; set; }
        public Guid ReelId { get; set; }
        public Guid ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal LockedQuantity { get; set; }
        public string Unit { get; set; }
        public decimal Weight { get; set; }
        public string BatchNo { get; set; }
        public string Source_WO { get; set; }
        public string SN { get; set; }
        public InventoryStatus Status { get; set; }
        public string? CraftVersion { get; set; }
        public DateTime FifoDate { get; set; }
        public int LayerIndex { get; set; }
        public string ReelNo { get; set; }
        public string LocationCode { get; set; }
        public string ProductName { get; set; }

        public string ProductCode { get; set; }
    }
}