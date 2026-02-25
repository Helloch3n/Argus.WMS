using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Argus.WMS.Outbound
{
    public class OutboundOrderItem : Entity<Guid>
    {
        public Guid OutboundOrderId { get; private set; }
        public string ProductCode { get; private set; }
        public decimal TargetLength { get; private set; }
        public int Quantity { get; private set; }
        public int AllocatedQuantity { get; internal set; }

        private OutboundOrderItem()
        {
        }

        public OutboundOrderItem(
            Guid id,
            Guid outboundOrderId,
            string productCode,
            decimal targetLength,
            int quantity)
            : base(id)
        {
            OutboundOrderId = outboundOrderId;
            ProductCode = Check.NotNullOrWhiteSpace(productCode, nameof(productCode));
            TargetLength = targetLength;
            Quantity = quantity;
            AllocatedQuantity = 0;
        }

        public void IncreaseAllocatedQuantity(int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            AllocatedQuantity += amount;
        }
    }
}