using Argus.WMS.Application.Contracts.Inventorys.Transaction.Dtos;
using Argus.WMS.Inventorys;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Argus.WMS.Application.Mappers
{
    [Mapper]
    public partial class InventoryTransactionMapper : MapperBase<InventoryTransaction, InventoryTransactionDto>
    {
        [MapProperty("Reel.ReelNo", nameof(InventoryTransactionDto.ReelNo))]
        [MapProperty("Product.Name", nameof(InventoryTransactionDto.ProductName))]
        [MapperIgnoreTarget(nameof(InventoryTransactionDto.FromLocationCode))]
        [MapperIgnoreTarget(nameof(InventoryTransactionDto.ToLocationCode))]
        [MapperIgnoreTarget(nameof(InventoryTransactionDto.FromWarehouseCode))]
        [MapperIgnoreTarget(nameof(InventoryTransactionDto.ToWarehouseCode))]
        private partial InventoryTransactionDto MapInternal(InventoryTransaction source);

        public override partial void Map(InventoryTransaction source, InventoryTransactionDto destination);

        public override InventoryTransactionDto Map(InventoryTransaction source)
        {
            var dto = MapInternal(source);
            dto.FromLocationCode = source.FromLocation?.Code;
            dto.ToLocationCode = source.ToLocation?.Code;
            dto.FromWarehouseCode = source.FromWarehouse?.Code;
            dto.ToWarehouseCode = source.ToWarehouse?.Code;
            return dto;
        }
    }
}