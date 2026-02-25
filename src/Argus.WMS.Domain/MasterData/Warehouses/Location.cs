using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Argus.WMS.MasterData.Warehouses
{
    public class Location : FullAuditedAggregateRoot<Guid>
    {
        public string Code { get; private set; }
        public string Aisle { get; private set; }
        public string Rack { get; private set; }
        public string Level { get; private set; }
        public string Bin { get; private set; }
        public Guid WarehouseId { get; private set; }

        public decimal MaxWeight { get; private set; }
        public decimal MaxVolume { get; private set; }
        public int MaxReelCount { get; private set; }

        public LocationStatus Status { get; private set; }
        public LocationType Type { get; private set; }

        /// <summary>
        /// 是否允许混放不同物料
        /// </summary>
        public bool AllowMixedProducts { get; private set; }

        /// <summary>
        /// 是否允许混放不同批次
        /// </summary>
        public bool AllowMixedBatches { get; private set; }

        public Guid ZoneId { get; private set; }

        protected Location()
        {
        }

        public Location(
            Guid id,
            Guid zoneId,
            string code,
            string aisle,
            string rack,
            string level,
            string bin,
            decimal maxWeight,
            decimal maxVolume,
            Guid warehouseId,
            int maxReelCount
            ) : base(id)
        {
            ZoneId = zoneId;
            Code = code;
            Aisle = aisle;
            Rack = rack;
            Level = level;
            Bin = bin;
            MaxWeight = maxWeight;
            MaxVolume = maxVolume;
            Status = LocationStatus.Idle;
            WarehouseId = warehouseId;
            AllowMixedProducts = true;
            AllowMixedBatches = true;
            MaxReelCount = maxReelCount;
        }

        public void Update(
            Guid zoneId,
            string code,
            string aisle,
            string rack,
            string level,
            string bin,
            decimal maxWeight,
            decimal maxVolume,
            int maxReelCount,
            Guid warehouseId,
            LocationType type,
            LocationStatus status,
            bool allowMixedProducts,
            bool allowMixedBatches)
        {
            ZoneId = zoneId;
            Code = code;
            Aisle = aisle;
            Rack = rack;
            Level = level;
            Bin = bin;
            MaxWeight = maxWeight;
            MaxVolume = maxVolume;
            MaxReelCount = maxReelCount;
            WarehouseId = warehouseId;
            Type = type;
            Status = status;
            AllowMixedProducts = allowMixedProducts;
            AllowMixedBatches = allowMixedBatches;
        }

        public void SetCoordinates(string aisle, string rack, string level, string bin)
        {
            Aisle = aisle;
            Rack = rack;
            Level = level;
            Bin = bin;
        }

        public void SetConstraints(decimal maxWeight, decimal maxVolume, int maxReelCount)
        {
            MaxWeight = maxWeight;
            MaxVolume = maxVolume;
            MaxReelCount = maxReelCount;
        }

        public void SetMixRules(bool allowMixedProducts, bool allowMixedBatches)
        {
            AllowMixedProducts = allowMixedProducts;
            AllowMixedBatches = allowMixedBatches;
        }

        public void ChangeStatus(LocationStatus newStatus)
        {
            Status = newStatus;
        }
    }
}
