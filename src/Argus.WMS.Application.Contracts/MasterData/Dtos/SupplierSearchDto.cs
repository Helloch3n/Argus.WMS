using Volo.Abp.Application.Dtos;

namespace Argus.WMS.MasterData.Dtos
{
    public class SupplierSearchDto : PagedAndSortedResultRequestDto
    {
        public string? SupplierCode { get; set; }
        public string? SupplierName { get; set; }
    }
}
