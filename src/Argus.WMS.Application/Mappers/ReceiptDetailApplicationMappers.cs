using Argus.WMS.Application.Contracts.Inbound.Dtos;
using Argus.WMS.Inbound;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Argus.WMS.Application.Mappers
{
    [Mapper]
    public partial class ReceiptDetailApplicationMappers : MapperBase<ReceiptDetail, ReceiptDetailDto>
    {
        [MapProperty("Product.Name", nameof(ReceiptDetailDto.ProductName))]
        [MapProperty("Reel.ReelNo", nameof(ReceiptDetailDto.ReelNo))]
        public override partial ReceiptDetailDto Map(ReceiptDetail source);

        public override partial void Map(ReceiptDetail source, ReceiptDetailDto destination);
    }
}