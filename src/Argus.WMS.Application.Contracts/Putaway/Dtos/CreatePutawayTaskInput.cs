using System.ComponentModel.DataAnnotations;

namespace Argus.WMS.Putaway.Dtos
{
    public class CreatePutawayTaskInput
    {
        [Required]
        public string ReelNo { get; set; }

        /// <summary> 计划目标库位（可选）。如果不填，则由系统策略推荐或后续人工扫描确认。</summary>
        public string? TargetLocationCode { get; set; }
    }
}