using System;

namespace Argus.WMS.Outbound
{
    public class AllocationResult
    {
        public Guid InventoryId { get; set; }
        public Guid ReelId { get; set; }
        public decimal AllocatedLength { get; set; }

        public AllocationResult(Guid inventoryId, Guid reelId, decimal allocatedLength)
        {
            InventoryId = inventoryId;
            ReelId = reelId;
            AllocatedLength = allocatedLength;
        }
    }
}
