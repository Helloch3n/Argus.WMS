using System;
using System.Collections.Generic;

namespace Argus.WMS.MasterData.Warehouses.Dtos
{
    public class WarehouseWithDetailsDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Manager { get; set; }
        public List<ZoneWithLocationsDto> Zones { get; set; } = [];
    }

    public class ZoneWithLocationsDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<LocationDto> Locations { get; set; } = [];
    }
}