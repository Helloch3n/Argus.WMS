using System;
using System.Threading.Tasks;
using Argus.WMS.MasterData.Reels;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Argus.WMS.Inventorys
{
    public class InventoryManager : DomainService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IRepository<Reel, Guid> _reelRepository;

        public InventoryManager(
            IInventoryRepository inventoryRepository,
            IRepository<Reel, Guid> reelRepository)
        {
            _inventoryRepository = inventoryRepository;
            _reelRepository = reelRepository;
        }

        public async Task<Inventory> ProductionReceiveAsync(
            Guid reelId,
            Guid productId,
            decimal quantity,
            decimal weight,
            string batchNo,
            string sourceWo,
            Guid locationId,
            string sn,
            string unit,
            string? craftVersion = null,
            int layerIndex = 0,
            InventoryStatus status = InventoryStatus.Good)
        {

            // SN ȫ��ΨһУ��
            var snExists = await _inventoryRepository.IsSnExistsAsync(sn);
            if (snExists)
            {
                throw new BusinessException("WMS:DuplicateSN")
                    .WithData("sn", sn);
            }
            var reel = await _reelRepository.GetAsync(reelId);

            reel.SetOccupied();
            reel.SetLocation(locationId);

            var maxLayer = await _inventoryRepository.GetMaxLayerOnReelAsync(reelId);
            var newLayer = layerIndex > 0 ? layerIndex : maxLayer + 1;

            var inventory = new Inventory(
                GuidGenerator.Create(),
                reelId,
                productId,
                quantity,
                unit,
                weight,
                batchNo,
                sourceWo,
                Clock.Now,
                newLayer,
                sn,
                craftVersion,
                status);

            await _inventoryRepository.InsertAsync(inventory, autoSave: true);
            await _reelRepository.UpdateAsync(reel, autoSave: true);

            return inventory;
        }
    }
}