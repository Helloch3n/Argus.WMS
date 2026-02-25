using System;
using Argus.WMS.Inventorys;

namespace Argus.WMS.Application.Contracts.Inventorys.Transaction.Dtos
{
    public class InventoryTransactionDto
    {
        public Guid Id { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public TransactionType Type { get; set; }
        public string BillNo { get; set; }
        public string Remark { get; set; }

        // 关联
        public string ReelNo { get; set; }
        public string ProductName { get; set; }

        // 库位 (From / To，可能为空)
        public string FromLocationCode { get; set; }
        public string ToLocationCode { get; set; }

        // 仓库 (From / To，可能为空)
        public string FromWarehouseCode { get; set; }
        public string ToWarehouseCode { get; set; }

        // 数量
        public decimal Quantity { get; set; }
        public decimal QuantityAfter { get; set; }

        // 追溯快照
        public string BatchNo { get; set; }
        public string SN { get; set; }
        public string CraftVersion { get; set; }
        public InventoryStatus Status { get; set; }
    }
}