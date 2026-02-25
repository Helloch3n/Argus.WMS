using System;
using Volo.Abp.Application.Dtos;

namespace Argus.WMS.Outbound.Dtos
{
    public class PickTaskDto : EntityDto<Guid>
    {
        public Guid OutboundOrderId { get; set; }
        public Guid OutboundOrderItemId { get; set; }
        public Guid InventoryId { get; set; }
        public string ReelNo { get; set; }
        public string? FromLocationCode { get; set; }
        public string? ToLocationCode { get; set; }
        public PickTaskStatus Status { get; set; }
        public DateTime CreationTime { get; set; }
        public string ProductCode { get; set; }
        public decimal TargetLength { get; set; }
        public string? BatchNo { get; set; }
        public string? Sn { get; set; }
    }
}