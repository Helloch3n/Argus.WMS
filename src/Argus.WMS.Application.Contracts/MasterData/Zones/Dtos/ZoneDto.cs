using System;
using Argus.WMS.MasterData.Warehouses;
using Volo.Abp.Application.Dtos;

namespace Argus.WMS.MasterData.Zones.Dtos
{
    public class ZoneDto : AuditedEntityDto<Guid>
    {
        public Guid WarehouseId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public ZoneType ZoneType { get; set; }
    }
}
