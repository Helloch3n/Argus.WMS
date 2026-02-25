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
        private readonly List<OutboundOrderItem> _items = new();
        public IReadOnlyCollection<OutboundOrderItem> Items => _items;

        protected OutboundOrder()
        {
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
        }

        public void AddItem(Guid id, string productCode, decimal targetLength, int quantity)
        {
            _items.Add(new OutboundOrderItem(id, Id, productCode, targetLength, quantity));
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