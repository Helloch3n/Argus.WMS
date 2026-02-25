using System.ComponentModel.DataAnnotations;

namespace Argus.WMS.DataSync.Dtos
{
    /// <summary>
    /// 创建/更新数据同步任务输入 DTO
    /// </summary>
    public class CreateUpdateDataSyncTaskDto
    {
        /// <summary>
        /// 任务编码（如 "SyncErpMaterial"）
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string TaskCode { get; set; }

        /// <summary>
        /// 任务名称（如 "ERP物料同步"）
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string TaskName { get; set; }

        /// <summary>
        /// Cron 表达式
        /// </summary>
        [Required]
        [MaxLength(32)]
        public string CronExpression { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}
