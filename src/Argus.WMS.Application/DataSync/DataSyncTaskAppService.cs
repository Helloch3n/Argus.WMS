using System;
using System.Threading.Tasks;
using Argus.WMS.DataSync.Dtos;
using Hangfire;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.DataSync
{
    /// <summary>
    /// 数据同步任务应用服务，负责 CRUD 并与 Hangfire 调度引擎联动
    /// </summary>
    public class DataSyncTaskAppService :
        CrudAppService<DataSyncTask, DataSyncTaskDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateDataSyncTaskDto>,
        IDataSyncTaskAppService
    {
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public DataSyncTaskAppService(
            IRepository<DataSyncTask, Guid> repository,
            IRecurringJobManager recurringJobManager,
            IBackgroundJobClient backgroundJobClient)
            : base(repository)
        {
            _recurringJobManager = recurringJobManager;
            _backgroundJobClient = backgroundJobClient;
        }

        /// <inheritdoc />
        public override async Task<DataSyncTaskDto> CreateAsync(CreateUpdateDataSyncTaskDto input)
        {
            var task = new DataSyncTask(
                GuidGenerator.Create(),
                input.TaskCode,
                input.TaskName,
                input.CronExpression);

            task.SetEnabled(input.IsEnabled);

            await Repository.InsertAsync(task, autoSave: true);

            SyncToHangfire(task);

            return ObjectMapper.Map<DataSyncTask, DataSyncTaskDto>(task);
        }

        /// <inheritdoc />
        public override async Task<DataSyncTaskDto> UpdateAsync(Guid id, CreateUpdateDataSyncTaskDto input)
        {
            var task = await Repository.GetAsync(id);

            task.SetTaskName(input.TaskName);
            task.UpdateSchedule(input.CronExpression);
            task.SetEnabled(input.IsEnabled);

            await Repository.UpdateAsync(task, autoSave: true);

            SyncToHangfire(task);

            return ObjectMapper.Map<DataSyncTask, DataSyncTaskDto>(task);
        }

        /// <inheritdoc />
        public override async Task DeleteAsync(Guid id)
        {
            var task = await Repository.GetAsync(id);

            await Repository.DeleteAsync(task);

            _recurringJobManager.RemoveIfExists(task.TaskCode);
        }

        /// <summary>
        /// 手动立即触发一次同步任务
        /// </summary>
        /// <param name="id">任务 ID</param>
        public async Task TriggerAsync(Guid id)
        {
            var task = await Repository.GetAsync(id);

            _backgroundJobClient.Enqueue<IDataSyncWorker>(w => w.ExecuteAsync(task.TaskCode));
        }

        /// <summary>
        /// 快捷启停同步任务
        /// </summary>
        /// <param name="id">任务 ID</param>
        /// <param name="isEnabled">是否启用</param>
        public async Task ToggleEnableAsync(Guid id, bool isEnabled)
        {
            var task = await Repository.GetAsync(id);

            task.SetEnabled(isEnabled);

            await Repository.UpdateAsync(task, autoSave: true);

            SyncToHangfire(task);
        }

        /// <summary>
        /// 将任务状态同步到 Hangfire 调度引擎
        /// </summary>
        private void SyncToHangfire(DataSyncTask task)
        {
            if (task.IsEnabled)
            {
                _recurringJobManager.AddOrUpdate<IDataSyncWorker>(
                    task.TaskCode,
                    w => w.ExecuteAsync(task.TaskCode),
                    task.CronExpression);
            }
            else
            {
                _recurringJobManager.RemoveIfExists(task.TaskCode);
            }
        }
    }
}
