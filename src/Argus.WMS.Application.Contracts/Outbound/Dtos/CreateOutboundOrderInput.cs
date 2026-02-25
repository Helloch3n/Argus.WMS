using System.Collections.Generic;

namespace Argus.WMS.Outbound.Dtos
{
    public class CreateOutboundOrderInput
    {
        public string? SourceOrderNo { get; set; }
        public string? CustomerName { get; set; }
        public List<CreateOutboundOrderItemInput> Items { get; set; } = new();
    }
}