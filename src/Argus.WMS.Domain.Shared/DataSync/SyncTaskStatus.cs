namespace Argus.WMS.DataSync
{
    /// <summary>
    /// 数据同步任务执行状态
    /// </summary>
    public enum SyncTaskStatus
    {
        /// <summary>
        /// 空闲/未执行
        /// </summary>
        Idle = 0,

        /// <summary>
        /// 执行中
        /// </summary>
        Running = 1,

        /// <summary>
        /// 成功
        /// </summary>
        Success = 2,

        /// <summary>
        /// 失败
        /// </summary>
        Failed = 3
    }
}
