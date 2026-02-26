using Volo.Abp.Application.Dtos;

namespace Argus.WMS.MasterData.Locations.Dtos
{
    public class LocationSearchDto : PagedAndSortedResultRequestDto
    {
        public string? ZoneCode { get; set; }
        public string? ZoneName { get; set; }
        public string? WarehouseCode { get; set; }
        public string? WarehouseName { get; set; }
        public string? LocationCode { get; set; }
    }
}
