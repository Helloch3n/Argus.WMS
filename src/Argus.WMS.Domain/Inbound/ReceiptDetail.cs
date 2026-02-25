using System;
using Argus.WMS.Inventorys;
using Argus.WMS.MasterData;
using Argus.WMS.MasterData.Reels;
using Volo.Abp.Domain.Entities.Auditing;

namespace Argus.WMS.Inbound
{
    public class ReceiptDetail : FullAuditedEntity<Guid>
    {
        public Guid ReceiptId { get; private set; }
        public Guid ReelId { get; private set; }
        public Guid ProductId { get; private set; }
        public decimal PlanQuantity { get; private set; }
        public string? BatchNo { get; private set; }
        public string? Source_WO { get; private set; }
        public bool IsReceived { get; private set; }
        public decimal ActualQuantity { get; private set; }
        public string Unit { get; private set; }
        // �����ֶ�
        public Guid? ToWarehouseId { get; private set; }
        public string SN { get; private set; }
        public string? CraftVersion { get; private set; }
        public int Layer_Index { get; private set; }
        public InventoryStatus Status { get; private set; }
        public decimal Weight { get; private set; }

        public Reel Reel { get; private set; }
        public Product Product { get; private set; }

        protected ReceiptDetail()
        {
        }

        public ReceiptDetail(
            Guid id,
            Guid receiptId,
            Guid reelId,
            Guid productId,
            decimal planQuantity,
            string unit,
            string sn,
            string? batchNo = null,
            string? sourceWo = null,
            Guid? toWarehouseId = null,
            string? craftVersion = null,
            int layerIndex = 1,
            InventoryStatus status = InventoryStatus.Good)
            : base(id)
        {
            ReceiptId = receiptId;
            ReelId = reelId;
            ProductId = productId;
            PlanQuantity = planQuantity;
            Unit = unit;
            SN = sn;
            BatchNo = batchNo;
            Source_WO = sourceWo;
            ToWarehouseId = toWarehouseId;
            CraftVersion = craftVersion;
            Layer_Index = layerIndex;
            Status = status;
            IsReceived = false;
            ActualQuantity = 0;
            Weight = 0;
        }

        public void Receive(decimal actualQuantity, decimal weight)
        {
            ActualQuantity = actualQuantity;
            Weight = weight;
            IsReceived = true;
        }

        public void SetSN(string sn)
        {
            SN = sn;
        }

        public void SetBatchNo(string? batchNo)
        {
            BatchNo = batchNo;
        }

        public void SetCraftVersion(string? craftVersion)
        {
            CraftVersion = craftVersion;
        }

        public void SetStatus(InventoryStatus status)
        {
            Status = status;
        }

        public void SetToWarehouseId(Guid? warehouseId)
        {
            ToWarehouseId = warehouseId;
        }

        public void SetActualQuantity(decimal actualQuantity)
        {
            ActualQuantity = actualQuantity;
        }
    }
}