using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Argus.WMS.DataSync
{
    /// <summary>
    /// 数据同步任务聚合根
    /// </summary>
    public class DataSyncTask : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 任务编码（如 "SyncErpMaterial"）
        /// </summary>
        public string TaskCode { get; private set; }

        /// <summary>
        /// 任务名称（如 "ERP物料同步"）
        /// </summary>
        public string TaskName { get; private set; }

        /// <summary>
        /// Cron 表达式
        /// </summary>
        public string CronExpression { get; private set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; private set; }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime? LastSyncTime { get; private set; }

        /// <summary>
        /// 上次执行状态
        /// </summary>
        public SyncTaskStatus LastSyncStatus { get; private set; }

        /// <summary>
        /// 上次执行日志或异常摘要
        /// </summary>
        public string? LastSyncMessage { get; private set; }

        protected DataSyncTask()
        {
        }

        public DataSyncTask(
            Guid id,
            string taskCode,
            string taskName,
            string cronExpression)
            : base(id)
        {
            TaskCode = Check.NotNullOrWhiteSpace(taskCode, nameof(taskCode));
            TaskName = Check.NotNullOrWhiteSpace(taskName, nameof(taskName));
            CronExpression = Check.NotNullOrWhiteSpace(cronExpression, nameof(cronExpression));
            IsEnabled = false;
            LastSyncStatus = SyncTaskStatus.Idle;
        }

        /// <summary>
        /// 更新任务名称
        /// </summary>
        public void SetTaskName(string taskName)
        {
            TaskName = Check.NotNullOrWhiteSpace(taskName, nameof(taskName));
        }

        /// <summary>
        /// 更新调度频率
        /// </summary>
        public void UpdateSchedule(string cronExpression)
        {
            CronExpression = Check.NotNullOrWhiteSpace(cronExpression, nameof(cronExpression));
        }

        /// <summary>
        /// 启停任务
        /// </summary>
        public void SetEnabled(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }

        /// <summary>
        /// 记录执行结果并更新上次执行时间
        /// </summary>
        public void RecordResult(SyncTaskStatus status, string? message = null)
        {
            LastSyncStatus = status;
            LastSyncMessage = message;
            LastSyncTime = DateTime.UtcNow;
        }
    }
}
