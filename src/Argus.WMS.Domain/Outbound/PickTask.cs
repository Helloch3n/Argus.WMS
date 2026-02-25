using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Argus.WMS.Outbound
{
    public class PickTask : FullAuditedAggregateRoot<Guid>
    {
        public Guid OutboundOrderId { get; private set; }
        public Guid OutboundOrderItemId { get; private set; }
        public Guid InventoryId { get; private set; }
        public string ReelNo { get; private set; }
        public string? FromLocation { get; private set; }
        public string? ToLocation { get; private set; }
        public PickTaskStatus Status { get; set; }

        private PickTask()
        {
        }

        public PickTask(
            Guid id,
            Guid outboundOrderId,
            Guid outboundOrderItemId,
            Guid inventoryId,
            string reelNo,
            string? fromLocation,
            string? toLocation)
            : base(id)
        {
            OutboundOrderId = outboundOrderId;
            OutboundOrderItemId = outboundOrderItemId;
            InventoryId = inventoryId;
            ReelNo = Check.NotNullOrWhiteSpace(reelNo, nameof(reelNo));
            FromLocation = fromLocation;
            ToLocation = toLocation;
            Status = PickTaskStatus.Pending;
        }

        public void MarkCompleted()
        {
            if (Status == PickTaskStatus.Completed)
            {
                return;
            }

            Status = PickTaskStatus.Completed;
        }
    }
}