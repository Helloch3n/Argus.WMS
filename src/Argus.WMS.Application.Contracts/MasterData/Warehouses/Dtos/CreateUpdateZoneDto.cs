using System;

namespace Argus.WMS.MasterData.Warehouses.Dtos
{
    public class CreateUpdateZoneDto
    {
        public Guid WarehouseId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public ZoneType ZoneType { get; set; }
    }
}
