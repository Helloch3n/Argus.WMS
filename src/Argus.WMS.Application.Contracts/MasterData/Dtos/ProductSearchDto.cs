using System;
using Volo.Abp.Application.Dtos;

namespace Argus.WMS.MasterData.Dtos
{
    public class ProductSearchDto : PagedAndSortedResultRequestDto
    {
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
    }
}
