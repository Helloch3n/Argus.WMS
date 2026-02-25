using System;
using Volo.Abp.Application.Dtos;

namespace Argus.WMS.MasterData.Dtos
{
    public class ProductDto : AuditedEntityDto<Guid>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }

        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Volume { get; set; }
        public decimal Weight { get; set; }

        public bool IsBatchManagementEnabled { get; set; }
        public int? ShelfLifeDays { get; set; }
    }
}