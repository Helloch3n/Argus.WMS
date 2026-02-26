using Argus.WMS.BillNumbers;
using Argus.WMS.Inventorys;
using Argus.WMS.MasterData.Locations;
using Argus.WMS.MasterData.Reels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Argus.WMS.Inbound
{
    public class ReceiptManager : DomainService
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IRepository<Inventory, Guid> _inventoryRepository;
        private readonly IRepository<Reel, Guid> _reelRepository;
        private readonly IRepository<Location, Guid> _locationRepository;
        private readonly IRepository<InventoryTransaction, Guid> _inventoryTransactionRepository;
        private readonly InventoryManager _inventoryManager;
        private readonly IBillNumberGenerator _billNumberGenerator;

        public ReceiptManager(
            IReceiptRepository receiptRepository,
            IRepository<Inventory, Guid> inventoryRepository,
            IRepository<InventoryTransaction, Guid> inventoryTransactionRepository,
            IRepository<Reel, Guid> reelRepository,
            IRepository<Location, Guid> locationRepository,
            InventoryManager inventoryManager,
            IBillNumberGenerator billNumberGenerator)
        {
            _reelRepository = reelRepository;
            _receiptRepository = receiptRepository;
            _inventoryRepository = inventoryRepository;
            _locationRepository = locationRepository;
            _inventoryTransactionRepository = inventoryTransactionRepository;
            _inventoryManager = inventoryManager;
            _billNumberGenerator = billNumberGenerator;
        }

        /// <summary>
        /// 执行入库核心逻辑
        /// </summary>
        public async Task ExecuteReceiptAsync(
            ReceiveInventoryArgs receiveInventoryArgs)
        {
            #region Step 1: 加载单据 & 前置校验

            var receipt = await _receiptRepository.GetWithDetailsAsync(receiveInventoryArgs.ReceiptId)
                ?? throw new UserFriendlyException("收货单不存在");

            var detail = receipt.Details.FirstOrDefault(x => x.Id == receiveInventoryArgs.DetailId)
                ?? throw new UserFriendlyException("收货明细不存在");

            if (detail.IsReceived)
            {
                throw new UserFriendlyException("该明细已收货，禁止重复收货");
            }

            var diff = receiveInventoryArgs.ActualQuantity - detail.PlanQuantity;
            if (detail.PlanQuantity > 0 &&
                diff / detail.PlanQuantity > 0.05m)
            {
                throw new UserFriendlyException("超收限制: 超出阈值禁止收货");
            }

            #endregion

            #region Step 2: 隐式移库（载具位置变更 & 跨仓库调拨流水）

            var queryReel = await _reelRepository.WithDetailsAsync(x => x.Inventorys);
            var reel = await AsyncExecuter.FirstOrDefaultAsync(queryReel.Where(x => x.Id == detail.ReelId));
            if (reel is null)
            {
                throw new UserFriendlyException("托盘不存在");
            }

            var targetLocation = await _locationRepository.GetAsync(receiveInventoryArgs.LocationId);

            bool isWarehouseChanged = false;
            if (reel.CurrentLocation != null && reel.CurrentLocation.WarehouseId != targetLocation.WarehouseId)
            {
                isWarehouseChanged = true;
            }

            reel.SetLocation(targetLocation.Id);

            if (isWarehouseChanged)
            {
                foreach (var o in reel.Inventorys)
                {
                    var transferLog = new InventoryTransaction(
                        GuidGenerator.Create(),
                        TransactionType.Transfer,
                        receipt.BillNo,
                        o.Id,
                        o.ReelId,
                        o.ProductId,
                        o.Quantity,
                        o.Quantity,
                        reel.CurrentLocation?.Id,
                        targetLocation.Id,
                        reel.CurrentLocation?.WarehouseId,
                        targetLocation.WarehouseId,
                        o.SN,
                        o.BatchNo,
                        o.CraftVersion,
                        o.Status,
                        $"入库自动纠偏：从 {reel.CurrentLocation?.WarehouseId} 移至 {targetLocation.WarehouseId}"
                    );

                    await _inventoryTransactionRepository.InsertAsync(transferLog);
                }
            }

            #endregion

            #region Step 3: 更新明细实绩 & 创建新库存

            var effectiveSn = receiveInventoryArgs.SN ?? detail.SN;
            if (string.IsNullOrWhiteSpace(effectiveSn))
            {
                throw new UserFriendlyException("SN（序列号/条码）不能为空，请在执行时补录");
            }

            if (!string.IsNullOrWhiteSpace(receiveInventoryArgs.SN) && receiveInventoryArgs.SN != detail.SN)
            {
                detail.SetSN(receiveInventoryArgs.SN);
            }

            var effectiveWeight = receiveInventoryArgs.Weight > 0 ? receiveInventoryArgs.Weight : receiveInventoryArgs.ActualWeight;

            detail.Receive(receiveInventoryArgs.ActualQuantity, effectiveWeight);
            receipt.SetStatus(ReceiptStatus.Receiving);

            var effectiveWarehouseId = detail.ToWarehouseId ?? receipt.WarehouseId;

            var inventory = await _inventoryManager.ProductionReceiveAsync(
                detail.ReelId,
                detail.ProductId,
                receiveInventoryArgs.ActualQuantity,
                effectiveWeight,
                detail.BatchNo,
                detail.Source_WO,
                receiveInventoryArgs.LocationId,
                effectiveSn,
                detail.Unit,
                detail.CraftVersion,
                detail.Layer_Index,
                detail.Status
                );

            #endregion

            #region Step 4: 记录入库流水 & 更新单据状态

            var transaction = new InventoryTransaction(
                GuidGenerator.Create(),
                TransactionType.Receipt,
                receipt.BillNo,
                inventory.Id,
                detail.ReelId,
                detail.ProductId,
                receiveInventoryArgs.ActualQuantity,
                receiveInventoryArgs.ActualQuantity,
                null,
                receiveInventoryArgs.LocationId,
                null,
                effectiveWarehouseId,
                effectiveSn,
                detail.BatchNo,
                detail.CraftVersion,
                detail.Status,
                string.Empty
            );

            await _inventoryTransactionRepository.InsertAsync(transaction, autoSave: true);

            if (receipt.Details.All(x => x.IsReceived))
            {
                receipt.SetStatus(ReceiptStatus.Completed);
            }

            await _receiptRepository.UpdateAsync(receipt, autoSave: true);

            #endregion
        }


        public async Task<Receipt> CreateAsync(Receipt receipt)
        {
            // 1. 调用接口生成单号 (Manager 不知道底层是用 Redis)
            var nextBillNo = await _billNumberGenerator.GetNextNumberAsync("ASN");
            receipt.SetBillNo(nextBillNo);

            // 2. 这里的 Create 逻辑通常包含数据校验
            if (await _receiptRepository.AnyAsync(x => x.BillNo == receipt.BillNo))
            {
                // 极低概率的防御性编程
                throw new UserFriendlyException("单号生成冲突，请重试");
            }

            return await _receiptRepository.InsertAsync(receipt, autoSave: true);
        }
    }
}