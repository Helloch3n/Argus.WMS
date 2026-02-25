using Argus.WMS.Application.Contracts.Inventorys.Dtos;
using Argus.WMS.Inventorys;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Argus.WMS.Application.Mappers
{
    [Mapper]
    public partial class InventoryApplicationMappers : MapperBase<Inventory, InventoryDto>
    {
        [MapProperty(nameof(Inventory.ProductId), nameof(InventoryDto.ProductId))]
        [MapProperty("Reel.ReelNo", nameof(InventoryDto.ReelNo))]
        [MapProperty("Reel.CurrentLocation.Code", nameof(InventoryDto.LocationCode))]
        [MapProperty("Product.Name", nameof(InventoryDto.ProductName))]
        [MapProperty("Product.Code", nameof(InventoryDto.ProductCode))]
        [MapProperty(nameof(Inventory.Index), nameof(InventoryDto.LayerIndex))]
        public override partial InventoryDto Map(Inventory source);

        public override partial void Map(Inventory source, InventoryDto destination);
    }
}