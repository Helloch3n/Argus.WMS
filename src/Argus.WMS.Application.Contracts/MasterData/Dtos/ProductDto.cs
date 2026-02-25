using System;
using Volo.Abp.Application.Dtos;

namespace Argus.WMS.MasterData.Dtos
{
    public class ProductDto : AuditedEntityDto<Guid>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string AuxUnit { get; set; }
        public decimal ConversionRate { get; set; }
        public bool IsBatchManagementEnabled { get; set; }
        public int? ShelfLifeDays { get; set; }
    }
}