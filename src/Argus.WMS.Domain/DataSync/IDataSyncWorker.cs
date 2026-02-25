using System.Threading.Tasks;

namespace Argus.WMS.DataSync
{
    /// <summary>
    /// 数据同步执行器接口，用于解耦调度框架（如 Hangfire）与具体的同步业务逻辑
    /// </summary>
    public interface IDataSyncWorker
    {
        /// <summary>
        /// 根据任务编码执行对应的数据同步逻辑
        /// </summary>
        /// <param name="taskCode">任务编码</param>
        Task ExecuteAsync(string taskCode);
    }
}
