using System;

namespace Argus.WMS.Putaway.Dtos
{
    public class PutawayReelItemDto
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string BatchNo { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
    }
}