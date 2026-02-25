using System;

namespace Argus.WMS.Outbound
{
    public class AllocationResult
    {
        public Guid InventoryId { get;  }
        public Guid ReelId { get;  }
        public decimal AllocatedLength { get;}

        public AllocationResult(Guid inventoryId, Guid reelId, decimal allocatedLength)
        {
            InventoryId = inventoryId;
            ReelId = reelId;
            AllocatedLength = allocatedLength;
        }
    }
}
