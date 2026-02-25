using Argus.WMS.Outbound;
using Argus.WMS.Outbound.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Argus.WMS.Application.Mappers
{
    [Mapper]
    public partial class OutboundOrderApplicationMappers : MapperBase<OutboundOrder, OutboundOrderDto>
    {
        public override partial OutboundOrderDto Map(OutboundOrder source);

        public override partial void Map(OutboundOrder source, OutboundOrderDto destination);

        private partial OutboundOrderItemDto MapItem(OutboundOrderItem source);
    }
}