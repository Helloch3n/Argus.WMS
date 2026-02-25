using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Argus.WMS.MasterData.Warehouses.Dtos
{
    public class WarehouseDto : AuditedEntityDto<Guid>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Manager { get; set; }
        public List<ZoneDto> Zones { get; set; } = new();
    }
}
