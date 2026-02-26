using System;
using Volo.Abp.Application.Dtos;

namespace Argus.WMS.Application.Contracts.Inventorys.Dtos
{
    public class InventorySearchDto : PagedAndSortedResultRequestDto
    {
        public string? ReelNo { get; set; }
        public Guid? ProductId { get; set; }
        public string? Source_WO { get; set; }
    }
}