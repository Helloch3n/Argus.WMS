using System;

namespace Argus.WMS.Application.Contracts.Inventorys.Dtos
{
    public class InventorySearchDto
    {
        public string? ReelNo { get; set; }
        public Guid? ProductId { get; set; }
        public string? Source_WO { get; set; }
    }
}