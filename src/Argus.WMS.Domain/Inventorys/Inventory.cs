using System;
using Argus.WMS.MasterData;
using Argus.WMS.MasterData.Reels;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Argus.WMS.Inventorys
{
    public class Inventory : FullAuditedAggregateRoot<Guid>
    {
        public Guid ReelId { get; private set; }
        public Guid ProductId { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal AvailableQuantity => Quantity - LockedQuantity;
        public decimal LockedQuantity { get; private set; }
        public string Unit { get; private set; }
        public decimal Weight { get; private set; }
        public string BatchNo { get; private set; }
        public string Source_WO { get; private set; }
        public string SN { get; private set; }
        public InventoryStatus Status { get; private set; }
        public string? CraftVersion { get; private set; }
        public DateTime FifoDate { get; private set; }
        public int Index { get; private set; }
        public Reel Reel { get; private set; }
        public Product Product { get; private set; }
        public string? RelatedOrderNo { get; private set; }

        protected Inventory()
        {
        }

        public Inventory(
            Guid id,
            Guid reelId,
            Guid productId,
            decimal quantity,
            string unit,
            decimal weight,
            string batchNo,
            string sourceWo,
            DateTime fifoDate,
            int layerIndex,
            string sn,
            string? craftVersion = null,
            InventoryStatus status = InventoryStatus.Good) : base(id)
        {
            ReelId = reelId;
            ProductId = productId;
            Quantity = quantity;
            LockedQuantity = 0;
            Unit = unit;
            Weight = weight;
            BatchNo = batchNo;
            Source_WO = sourceWo;
            SN = Check.NotNullOrWhiteSpace(sn, nameof(sn), maxLength: 100);
            Status = status;
            CraftVersion = craftVersion;
            FifoDate = fifoDate;
            Index = layerIndex;
        }

        /// <summary>
        /// 锁定库存（逻辑锁定）
        /// </summary>
        public void LockQuantity(decimal targetLength)
        {
            if (AvailableQuantity < targetLength)
                throw new InvalidOperationException($"可用数量不足。需要: {targetLength}，可用: {AvailableQuantity}");

            LockedQuantity += targetLength;
        }

        /// <summary>
        /// 预留库存（Available -> Locked）
        /// </summary>
        public void Reserve(decimal amount)
        {
            if (amount <= 0)
                throw new UserFriendlyException("预留数量必须大于0。");

            if (AvailableQuantity < amount)
                throw new UserFriendlyException(
                    $"可用数量不足，当前可用: {AvailableQuantity}，需要预留: {amount}");

            LockedQuantity += amount;
        }

        /// <summary>
        /// 释放预留（Locked -> Available）
        /// </summary>
        public void Unreserve(decimal amount)
        {
            if (amount <= 0)
                throw new UserFriendlyException("释放数量必须大于0。");

            if (LockedQuantity < amount)
                throw new UserFriendlyException(
                    $"锁定数量不足，当前锁定: {LockedQuantity}，需要释放: {amount}");

            LockedQuantity -= amount;
        }

        /// <summary>
        /// 拣货完成扣减（同时扣减总量与锁定量）
        /// </summary>
        public void DeductLockedQuantity(decimal amount)
        {
            if (amount <= 0)
            {
                throw new UserFriendlyException("扣减数量必须大于0。");
            }
            if (LockedQuantity < amount)
            {
                throw new UserFriendlyException(
                    $"锁定数量不足，当前锁定: {LockedQuantity}，需要扣减: {amount}");
            }
            if (Quantity < amount)
            {
                throw new UserFriendlyException(
                    $"库存数量不足，当前库存: {Quantity}，需要扣减: {amount}");
            }
            LockedQuantity -= amount;
            Quantity -= amount;
        }

        /// <summary>
        /// 变更库存状态
        /// </summary>
        public void ChangeStatus(InventoryStatus newStatus)
        {
            if (Status == newStatus)
                return;

            Status = newStatus;
        }

        /// <summary>
        /// 按数量扣减，优先扣减锁定量，其余扣减可用量
        /// </summary>
        public void DecreaseQuantity(decimal amount)
        {
            if (amount <= 0)
                throw new UserFriendlyException("扣减数量必须大于0。");

            if (amount > Quantity)
                throw new UserFriendlyException(
                    $"库存数量不足，当前库存: {Quantity}，需要扣减: {amount}");

            var fromLocked = Math.Min(LockedQuantity, amount);
            LockedQuantity -= fromLocked;

            var fromAvailable = amount - fromLocked;
            Quantity -= amount;
        }
    }
}