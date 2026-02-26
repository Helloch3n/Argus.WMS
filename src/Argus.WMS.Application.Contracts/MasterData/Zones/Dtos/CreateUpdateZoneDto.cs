using System;
using Argus.WMS.MasterData.Warehouses;

namespace Argus.WMS.MasterData.Zones.Dtos
{
    public class CreateUpdateZoneDto
    {
        public Guid WarehouseId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public ZoneType ZoneType { get; set; }
    }
}
