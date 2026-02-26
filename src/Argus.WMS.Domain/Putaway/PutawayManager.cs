using System;
using System.Threading.Tasks;
using Argus.WMS.MasterData.Locations;
using Argus.WMS.MasterData.Reels;
using Argus.WMS.MasterData.Warehouses;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Argus.WMS.Putaway
{
    public class PutawayManager : DomainService
    {
        private readonly IReelRepository _reelRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IRepository<PutawayTask, Guid> _putawayTaskRepository;

        public PutawayManager(
            IReelRepository reelRepository,
            ILocationRepository locationRepository,
            IRepository<PutawayTask, Guid> putawayTaskRepository)
        {
            _reelRepository = reelRepository;
            _locationRepository = locationRepository;
            _putawayTaskRepository = putawayTaskRepository;
        }

        public async Task<PutawayTask> CreateTaskAsync(string reelNo)
        {
            var reel = await _reelRepository.GetByReelNoWithLocationAsync(reelNo);
            if (reel is null)
            {
                throw new UserFriendlyException("盘具不存在");
            }

            if (reel.Status is ReelStatus.Damaged or ReelStatus.Maintenance)
            {
                throw new UserFriendlyException("盘具状态不可上架");
            }

            if (reel.Status != ReelStatus.Occupied)
            {
                throw new UserFriendlyException("盘具状态必须为有货");
            }

            if (reel.IsLocked)
            {
                throw new UserFriendlyException("盘具正在操作中");
            }

            if (reel.CurrentLocation is null || reel.CurrentLocation.Type != LocationType.Dock)
            {
                throw new UserFriendlyException("盘具当前不在暂存区");
            }

            reel.Lock("Putaway");
            await _reelRepository.UpdateAsync(reel);

            var task = new PutawayTask(
                GuidGenerator.Create(),
                GuidGenerator.Create().ToString("N"),
                reel.ReelNo,
                reel.CurrentLocation.Code,
                null,
                null);

            await _putawayTaskRepository.InsertAsync(task);

            return task;
        }

        public async Task CompleteTaskAsync(Guid id, string targetLocationCode)
        {
            var task = await _putawayTaskRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (task is null)
            {
                throw new UserFriendlyException("任务不存在");
            }

            await CompleteTaskAsync(task, targetLocationCode);
        }

        public async Task<PutawayTask> CreateTaskAsync(string reelNo, string? targetLocationCode = null)
        {
            var reel = await _reelRepository.GetByReelNoWithLocationAsync(reelNo);
            if (reel is null)
            {
                throw new UserFriendlyException("盘具不存在");
            }

            if (reel.Status is ReelStatus.Damaged or ReelStatus.Maintenance)
            {
                throw new UserFriendlyException("盘具状态不可上架");
            }

            if (reel.Status != ReelStatus.Occupied)
            {
                throw new UserFriendlyException("盘具状态必须为有货");
            }

            if (reel.IsLocked)
            {
                throw new UserFriendlyException("盘具正在操作中");
            }

            if (reel.CurrentLocation is null || reel.CurrentLocation.Type != LocationType.Dock)
            {
                throw new UserFriendlyException("盘具当前不在暂存区");
            }

            Location? targetLocation = null;
            if (!string.IsNullOrWhiteSpace(targetLocationCode))
            {
                targetLocation = await _locationRepository.GetByCodeAsync(targetLocationCode);
                if (targetLocation is null)
                {
                    throw new UserFriendlyException("目标库位不存在");
                }

                if (targetLocation.Status is not (LocationStatus.Full or LocationStatus.Locked))
                {
                    throw new UserFriendlyException("目标库位状态不可上架");
                }
            }

            reel.Lock("Putaway");
            await _reelRepository.UpdateAsync(reel);

            var task = new PutawayTask(
                GuidGenerator.Create(),
                GuidGenerator.Create().ToString("N"),
                reel.ReelNo,
                reel.CurrentLocation.Code,
                targetLocation?.Code,
                null);

            await _putawayTaskRepository.InsertAsync(task);

            return task;
        }

        public async Task CompleteTaskAsync(PutawayTask task, string targetLocationCode)
        {
            if (task.Status is not (PutawayTaskStatus.InProgress or PutawayTaskStatus.Pending))
            {
                throw new UserFriendlyException("任务状态不可完成");
            }

            var reel = await _reelRepository.FirstOrDefaultAsync(x => x.ReelNo == task.ReelNo);
            if (reel is null)
            {
                throw new UserFriendlyException("盘具不存在");
            }

            var targetLocation = await _locationRepository.GetByCodeAsync(targetLocationCode);
            if (targetLocation is null)
            {
                throw new UserFriendlyException("目标库位不存在");
            }

            reel.SetLocation(targetLocation.Id);
            reel.UnLock();
            task.Complete();

            await _reelRepository.UpdateAsync(reel);
            await _putawayTaskRepository.UpdateAsync(task);
        }
    }
}