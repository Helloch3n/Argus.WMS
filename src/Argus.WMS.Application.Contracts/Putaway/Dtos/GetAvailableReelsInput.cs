using System;
using Volo.Abp.Application.Dtos;

namespace Argus.WMS.Putaway.Dtos
{
    public class GetAvailableReelsInput : PagedAndSortedResultRequestDto
    {
        public Guid? WarehouseId { get; set; }
        public string Filter { get; set; } = string.Empty;
    }
}