using Argus.WMS.Inbound;
using Argus.WMS.Inventorys;
using Argus.WMS.MasterData.Warehouses;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Argus.WMS.MasterData.Reels
{
    public class Reel : FullAuditedAggregateRoot<Guid>
    {
        public string ReelNo { get; private set; }
        public string Name { get; private set; }
        public ReelType Type { get; private set; }
        public decimal SelfWeight { get; private set; }
        public decimal MaxWeight { get; private set; }
        public ReelStatus Status { get; private set; }
        public bool IsLocked { get; private set; }
        public Guid? CurrentLocationId { get; private set; }
        public Location CurrentLocation { get; private set; }
        public List<Inventory> Inventorys { get; private set; }

        private Reel()
        {
        }

        public Reel(
            Guid id,
            string reelNo,
            string name,
            ReelType type,
            decimal selfWeight,
            decimal maxWeight,
            ReelStatus status,
            Guid? currentLocationId) : base(id)
        {
            ReelNo = reelNo;
            Name = name;
            Type = type;
            SelfWeight = selfWeight;
            MaxWeight = maxWeight;
            Status = status;
            CurrentLocationId = currentLocationId;
            IsLocked = false;
        }

        public void SetLocation(Guid? locationId)
        {
            CurrentLocationId = locationId;
        }

        public void SetOccupied()
        {
            Status = ReelStatus.Occupied;
        }

        public void SetEmpty()
        {
            Status = ReelStatus.Empty;
        }

        public void Lock(string reason)
        {
            IsLocked = true;
        }

        public void UnLock()
        {
            IsLocked = false;
        }

        public void Update(
            string reelNo,
            string name,
            ReelType type,
            decimal selfWeight,
            decimal maxWeight)
        {
            ReelNo = reelNo;
            Name = name;
            Type = type;
            SelfWeight = selfWeight;
            MaxWeight = maxWeight;
        }
    }
}