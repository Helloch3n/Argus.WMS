using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Argus.WMS.MasterData.Warehouses
{
    public class Warehouse : FullAuditedAggregateRoot<Guid>
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Manager { get; private set; }

        public ICollection<Zone> Zones { get; private set; }

        private Warehouse()
        {
            Zones = new List<Zone>();
        }

        public Warehouse(
            Guid id,
            string code,
            string name,
            string address,
            string manager) : base(id)
        {
            Code = code;
            Name = name;
            Address = address;
            Manager = manager;
            Zones = new List<Zone>();
        }

        public void Update(string code, string name, string address, string manager)
        {
            Code = code;
            Name = name;
            Address = address;
            Manager = manager;
        }
    }
}
