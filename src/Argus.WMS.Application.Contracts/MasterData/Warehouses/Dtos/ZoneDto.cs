using System;
using Volo.Abp.Application.Dtos;

namespace Argus.WMS.MasterData.Warehouses.Dtos
{
    public class ZoneDto : AuditedEntityDto<Guid>
    {
        public Guid WarehouseId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public ZoneType ZoneType { get; set; }
    }
}
