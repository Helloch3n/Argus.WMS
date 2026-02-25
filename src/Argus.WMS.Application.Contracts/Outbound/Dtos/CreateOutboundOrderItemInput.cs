namespace Argus.WMS.Outbound.Dtos
{
    public class CreateOutboundOrderItemInput
    {
        public string ProductCode { get; set; }
        public decimal TargetLength { get; set; }
        public int Quantity { get; set; }
    }
}