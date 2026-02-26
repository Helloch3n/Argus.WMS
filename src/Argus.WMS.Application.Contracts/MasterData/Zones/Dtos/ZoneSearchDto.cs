using Volo.Abp.Application.Dtos;

namespace Argus.WMS.MasterData.Zones.Dtos
{
    public class ZoneSearchDto : PagedAndSortedResultRequestDto
    {
        public string? ZoneCode { get; set; }
        public string? ZoneName { get; set; }
        public string? WarehouseCode { get; set; }
        public string? WarehouseName { get; set; }
    }
}
