using Argus.WMS.Application.Contracts.Inbound.Dtos;
using Argus.WMS.Inbound;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Argus.WMS.Application.Mappers
{
    [Mapper]
    public partial class ReceiptApplicationMappers : MapperBase<Receipt, ReceiptDto>
    {
        public override partial ReceiptDto Map(Receipt source);

        public override partial void Map(Receipt source, ReceiptDto destination);

        [MapProperty(nameof(ReceiptDetail.Reel.ReelNo), nameof(ReceiptDetailDto.ReelNo))]
        private partial ReceiptDetailDto MapDetail(ReceiptDetail source);
    }
}