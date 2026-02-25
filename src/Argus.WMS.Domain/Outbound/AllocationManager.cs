using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.Inventorys;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Argus.WMS.Outbound
{
    public class AllocationManager : DomainService
    {
        private readonly IInventoryRepository _inventoryRepository;

        public AllocationManager(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<List<AllocationResult>> AllocateAsync(string productCode, decimal targetLength, int quantity)
        {
            var validInventories = await _inventoryRepository.GetAllocatableInventoriesAsync(productCode, targetLength);

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