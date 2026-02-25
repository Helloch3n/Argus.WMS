using System;

namespace Argus.WMS.Application.Contracts.Inbound.Dtos
{
    public class ExecuteReceiptInput
    {
        public Guid ReceiptId { get; set; }
        public Guid DetailId { get; set; }
        public decimal ActualQuantity { get; set; }
        public decimal ActualWeight { get; set; }
        public Guid LocationId { get; set; }

        // ÐÂÔö×Ö¶Î
        public string? SN { get; set; }
        public decimal Weight { get; set; }
    }
}