using System;
using System.Threading.Tasks;
using Argus.WMS.DataSync.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Argus.WMS.DataSync
{
    /// <summary>
    /// 数据同步任务应用服务接口
    /// </summary>
    public interface IDataSyncTaskAppService : ICrudAppService<
        DataSyncTaskDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateDataSyncTaskDto>
    {
        /// <summary>
        /// 手动立即触发一次同步任务
        /// </summary>
        /// <param name="id">任务 ID</param>
        Task TriggerAsync(Guid id);

        /// <summary>
        /// 快捷启停同步任务
        /// </summary>
        /// <param name="id">任务 ID</param>
        /// <param name="isEnabled">是否启用</param>
        Task ToggleEnableAsync(Guid id, bool isEnabled);
    }
}
