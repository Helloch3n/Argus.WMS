using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Argus.WMS.MasterData.Warehouses
{
    public class Zone : FullAuditedAggregateRoot<Guid>
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public ZoneType ZoneType { get; private set; }

        public Guid WarehouseId { get; private set; }

        protected Zone()
        {
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
