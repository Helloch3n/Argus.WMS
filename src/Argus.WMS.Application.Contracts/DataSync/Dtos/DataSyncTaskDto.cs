using System;
using Argus.WMS.DataSync;
using Volo.Abp.Application.Dtos;

namespace Argus.WMS.DataSync.Dtos
{
    /// <summary>
    /// 数据同步任务输出 DTO
    /// </summary>
    public class DataSyncTaskDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 任务编码
        /// </summary>
        public string TaskCode { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// Cron 表达式
        /// </summary>
        public string CronExpression { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime? LastSyncTime { get; set; }

        /// <summary>
        /// 上次执行状态
        /// </summary>
        public SyncTaskStatus LastSyncStatus { get; set; }

        /// <summary>
        /// 上次执行日志或异常摘要
        /// </summary>
        public string? LastSyncMessage { get; set; }
    }
}
