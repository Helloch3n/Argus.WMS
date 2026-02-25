using System;
using Argus.WMS.Inventorys;

namespace Argus.WMS.Application.Contracts.Inbound.Dtos
{
    public class ReceiptDetailDto
    {
        public Guid Id { get; set; }
        public Guid ReceiptId { get; set; }
        public Guid ReelId { get; set; }
        public string? ReelNo { get; set; }
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal PlanQuantity { get; set; }
        public string? BatchNo { get; set; }    
        public string? Source_WO { get; set; }
        public bool IsReceived { get; set; }
        public decimal ActualQuantity { get; set; }

        // ÐÂÔö×Ö¶Î
        public Guid? ToWarehouseId { get; set; }
        public string SN { get; set; }
        public string? CraftVersion { get; set; }
        public int Layer_Index { get; set; }
        public InventoryStatus Status { get; set; }
        public decimal Weight { get; set; }
    }
}