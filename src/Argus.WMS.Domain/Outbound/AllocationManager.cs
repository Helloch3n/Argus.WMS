using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.Inventorys;
using Argus.WMS.MasterData.Reels;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Argus.WMS.Outbound
{
    public class AllocationManager : DomainService
    {
        private readonly IRepository<Inventory, Guid> _inventoryRepository;

        public AllocationManager(IRepository<Inventory, Guid> inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<List<AllocationResult>> AllocateAsync(string productCode, decimal targetLength, int quantity)
        {
            var query = await _inventoryRepository.WithDetailsAsync(x => x.Reel, x => x.Product);

            var dbCandidates = query
                .Where(x => x.Product.Code == productCode)
                .Where(x => x.Reel.Status == ReelStatus.Occupied)
                .Where(x => !x.Reel.IsLocked);

            var allCandidates = await AsyncExecuter.ToListAsync(dbCandidates);

            var validInventories = allCandidates
                .GroupBy(x => x.ReelId)
                .Select(g => g.OrderByDescending(x => x.Index).First())
                .Where(x => x.AvailableQuantity >= targetLength)
                .OrderBy(x => x.CreationTime)
                .ToList();

            var results = new List<AllocationResult>();

            for (int i = 0; i < quantity; i++)
            {
                var selectedInventory = validInventories.FirstOrDefault(x => x.AvailableQuantity >= targetLength);

                if (selectedInventory == null)
                {
                    throw new UserFriendlyException($"库存不足！产品[{productCode}]还需要{quantity - i}盘长度为{targetLength}的库存，未找到满足条件的盘具。");
                }

                selectedInventory.LockQuantity(targetLength);

                results.Add(new AllocationResult(selectedInventory.Id, selectedInventory.ReelId, targetLength));
            }

            var updatedInventories = results.Select(r => r.InventoryId).Distinct()
                .Select(id => validInventories.First(x => x.Id == id))
                .ToList();

            if (updatedInventories.Any())
            {
                await _inventoryRepository.UpdateManyAsync(updatedInventories, autoSave: true);
            }

            return results;
        }
    }
}