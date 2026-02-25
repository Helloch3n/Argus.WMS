using System;
using Volo.Abp.Application.Dtos;

namespace Argus.WMS.MasterData.Warehouses.Dtos
{
    public class LocationDto : AuditedEntityDto<Guid>
    {
        public Guid ZoneId { get; set; }
        public Guid WarehouseId { get; set; }
        public string Code { get; set; }
        public string Aisle { get; set; }
        public string Rack { get; set; }
        public string Level { get; set; }
        public string Bin { get; set; }
        public decimal MaxWeight { get; set; }
        public decimal MaxVolume { get; set; }
        public int MaxReelCount { get; set; }
        public LocationType Type { get; set; }
        public LocationStatus Status { get; set; }
        public bool AllowMixedProducts { get; set; }
        public bool AllowMixedBatches { get; set; }
    }
}
