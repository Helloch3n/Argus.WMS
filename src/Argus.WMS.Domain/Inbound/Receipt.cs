using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Argus.WMS.Inbound
{
    public class Receipt : FullAuditedAggregateRoot<Guid>
    {
        public string BillNo { get; private set; }
        public ReceiptType Type { get; private set; }
        public ReceiptStatus Status { get; private set; }
        public Guid WarehouseId { get; private set; }

        private readonly List<ReceiptDetail> _details = new();
        public IReadOnlyCollection<ReceiptDetail> Details => _details;

        protected Receipt()
        {
        }

        public Receipt(
            Guid id,
            ReceiptType type,
            ReceiptStatus status,
            Guid warehouseId,
            string billNo) : base(id)
        {
            Type = type;
            Status = status;
            WarehouseId = warehouseId;
            BillNo = billNo;
        }

        public void SetStatus(ReceiptStatus status)
        {
            Status = status;
        }

        public void SetBillNo(string billNo)
        {
            BillNo = Check.NotNullOrWhiteSpace(billNo, nameof(billNo));
        }

        public void AddDetail(ReceiptDetail detail)
        {
            _details.Add(detail);
        }
    }
}