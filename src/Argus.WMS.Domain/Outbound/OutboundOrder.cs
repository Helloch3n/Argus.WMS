using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Argus.WMS.Outbound
{
    public class OutboundOrder : FullAuditedAggregateRoot<Guid>
    {
        public string OrderNo { get; private set; }
        public string? SourceOrderNo { get; private set; }
        public string? CustomerName { get; private set; }
        public OutboundOrderStatus Status { get; private set; }
        public ICollection<OutboundOrderItem> Items { get; private set; }

        private OutboundOrder()
        {
            Items = new List<OutboundOrderItem>();
        }

        public OutboundOrder(
            Guid id,
            string orderNo,
            string? sourceOrderNo,
            string? customerName)
            : base(id)
        {
            OrderNo = Check.NotNullOrWhiteSpace(orderNo, nameof(orderNo));
            SourceOrderNo = sourceOrderNo;
            CustomerName = customerName;
            Status = OutboundOrderStatus.Created;
            Items = new List<OutboundOrderItem>();
        }

        public void AddItem(Guid id, string productCode, decimal targetLength, int quantity)
        {
            Items.Add(new OutboundOrderItem(id, Id, productCode, targetLength, quantity));
        }

        public void MarkAllocated()
        {
            Status = OutboundOrderStatus.Allocated;
        }

        public void MarkPartiallyAllocated()
        {
            Status = OutboundOrderStatus.PartiallyAllocated;
        }

        public void MarkCompleted()
        {
            Status = OutboundOrderStatus.Completed;
        }
    }
}