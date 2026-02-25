using System;
using Argus.WMS.Inventorys;
using Volo.Abp.Application.Dtos;

namespace Argus.WMS.Application.Contracts.Inventorys.Transaction.Dtos
{
    public class InventoryTransactionSearchDto : PagedAndSortedResultRequestDto
    {
        public string? BillNo { get; set; }
        public string? ReelNo { get; set; }
        public Guid? ProductId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TransactionType? Type { get; set; }
    }
}