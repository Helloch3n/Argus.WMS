using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Argus.WMS.MasterData.Warehouses
{
    public class Zone : FullAuditedEntity<Guid>
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public ZoneType ZoneType { get; private set; }

        public Guid WarehouseId { get; private set; }
        public ICollection<Location> Locations { get; private set; }

        private Zone()
        {
            Locations = new List<Location>();
        }

        public Zone(
            Guid id,
            Guid warehouseId,
            string code,
            string name,
            ZoneType zoneType) : base(id)
        {
            WarehouseId = warehouseId;
            Code = code;
            Name = name;
            ZoneType = zoneType;
            Locations = new List<Location>();
        }

        public void Update(Guid warehouseId, string code, string name, ZoneType zoneType)
        {
            WarehouseId = warehouseId;
            Code = code;
            Name = name;
            ZoneType = zoneType;
        }
    }
}
