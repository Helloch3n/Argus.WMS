using System;
using System.Collections.Generic;
using Argus.WMS.Inbound;

namespace Argus.WMS.Application.Contracts.Inbound.Dtos
{
    public class ReceiptDto
    {
        public Guid Id { get; set; }
        public string BillNo { get; set; }  
        public ReceiptType Type { get; set; }
        public ReceiptStatus Status { get; set; }
        public string SourceBillNo { get; set; }
        public Guid WarehouseId { get; set; }
        public DateTime CreationTime { get; set; }

        public List<ReceiptDetailDto> Details { get; set; } = new();
    }
}