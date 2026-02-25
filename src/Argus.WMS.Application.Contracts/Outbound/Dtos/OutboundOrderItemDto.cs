namespace Argus.WMS.Outbound.Dtos
{
    public class OutboundOrderItemDto
    {
        public string ProductCode { get; set; }
        public decimal TargetLength { get; set; }
        public int Quantity { get; set; }
        public int AllocatedQuantity { get; set; }
    }
}