using Volo.Abp.Application.Dtos;

namespace Argus.WMS.MasterData.Warehouses.Dtos
{
    public class WarehouseSearchDto : PagedAndSortedResultRequestDto
    {
        public string? WarehouseCode { get; set; }
        public string? WarehouseName { get; set; }
    }
}
