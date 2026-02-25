using System;
using System.Collections.Generic;

namespace Argus.WMS.Outbound.Dtos
{
    public class OutboundOrderDto
    {
        public Guid Id { get; set; }
        public string OrderNo { get; set; }
        public string? SourceOrderNo { get; set; }
        public string? CustomerName { get; set; }
        public OutboundOrderStatus Status { get; set; }
        public DateTime CreationTime { get; set; }
        public List<OutboundOrderItemDto> Items { get; set; } = new();
    }
}