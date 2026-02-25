using System;
using Volo.Abp.Application.Dtos;

namespace Argus.WMS.MasterData.Warehouses.Dtos
{
    public class WarehouseDto : AuditedEntityDto<Guid>
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
