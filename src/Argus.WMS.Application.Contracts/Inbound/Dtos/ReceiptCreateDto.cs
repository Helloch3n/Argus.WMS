using System;
using System.Collections.Generic;
using Argus.WMS.Inbound;
using Argus.WMS.Inventorys;

namespace Argus.WMS.Application.Contracts.Inbound.Dtos
{
    public class ReceiptCreateDto
    {
        public ReceiptType Type { get; set; }
        public Guid WarehouseId { get; set; }
        public string BillNo { get; set; } = string.Empty;
        public List<ReceiptDetailCreateDto> Details { get; set; } = new();
    }

    public class ReceiptDetailCreateDto
    {
        public Guid ReelId { get; set; }
        public Guid ProductId { get; set; }
        public decimal PlanQuantity { get; set; }
        public string Unit { get; set; }
        public string? BatchNo { get; set; }
        public string? Source_WO { get; set; }

        // ÐÂÔö×Ö¶Î
        public string SN { get; set; }
        public string? CraftVersion { get; set; }
        public int Layer_Index { get; set; } = 1;
        public Guid? ToWarehouseId { get; set; }
        public InventoryStatus Status { get; set; } = InventoryStatus.Good;
    }
}