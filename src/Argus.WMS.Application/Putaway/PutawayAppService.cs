using System;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.Inventorys;
using Argus.WMS.MasterData.Locations;
using Argus.WMS.MasterData.Reels;
using Argus.WMS.MasterData.Reels.Dtos;
using Argus.WMS.Putaway.Dtos;
using System.Linq.Dynamic.Core;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using System.Collections.Generic;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.Putaway
{
    public class PutawayAppService :
        CrudAppService<PutawayTask, PutawayTaskDto, Guid, GetPutawayTaskListInput, CreatePutawayTaskInput>,
        IPutawayAppService
    {
        private readonly PutawayManager _putawayManager;
        private readonly IReelRepository _reelRepository;
        private readonly IRepository<Inventory, Guid> _inventoryRepository;
        private readonly IRepository<Location, Guid> _locationRepository;

        public PutawayAppService(
            PutawayManager putawayManager,
            IReelRepository reelRepository,
            IRepository<Inventory, Guid> inventoryRepository,
            IRepository<Location, Guid> locationRepository,
            IRepository<PutawayTask, Guid> putawayTaskRepository)
            : base(putawayTaskRepository)
        {
            _putawayManager = putawayManager;
            _reelRepository = reelRepository;
            _inventoryRepository = inventoryRepository;
            _locationRepository = locationRepository;
        }

        public override async Task<PutawayTaskDto> CreateAsync(CreatePutawayTaskInput input)
        {
            var task = await _putawayManager.CreateTaskAsync(input.ReelNo, input.TargetLocationCode);
            return ObjectMapper.Map<PutawayTask, PutawayTaskDto>(task);
        }

        public async Task CompleteAsync(Guid id, CompletePutawayTaskInput input)
        {
            await _putawayManager.CompleteTaskAsync(id, input.TargetLocationCode);
        }

        public override async Task<PagedResultDto<PutawayTaskDto>> GetListAsync(GetPutawayTaskListInput input)
        {
            var query = await Repository.GetQueryableAsync();

            if (input.Status.HasValue)
            {
                query = query.Where(x => x.Status == input.Status.Value);
            }

            var totalCount = await AsyncExecuter.CountAsync(query);

            var sorting = string.IsNullOrWhiteSpace(input.Sorting)
                ? "CreationTime DESC"
                : input.Sorting;

            var tasks = await AsyncExecuter.ToListAsync(
                query
                    .OrderBy(sorting)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount));

            var dtos = tasks.Select(MapPutawayTask).ToList();

            return new PagedResultDto<PutawayTaskDto>(totalCount, dtos);
        }

        public async Task<PagedResultDto<PutawayReelDto>> GetAvailableReelsAsync(GetAvailableReelsInput input)
        {
            var totalCount = await _reelRepository.GetAvailableForPutawayCountAsync(
                input.Filter,
                input.WarehouseId);

            var reels = await _reelRepository.GetAvailableForPutawayListAsync(
                input.Filter,
                input.WarehouseId,
                input.Sorting,
                input.SkipCount,
                input.MaxResultCount);

            var reelIds = reels.Select(x => x.Id).ToList();

            var inventoryQuery = await _inventoryRepository.WithDetailsAsync(x => x.Product);
            var inventories = await AsyncExecuter.ToListAsync(inventoryQuery.Where(x => reelIds.Contains(x.ReelId)));

            var locationIds = reels
                .Where(x => x.CurrentLocationId.HasValue)
                .Select(x => x.CurrentLocationId!.Value)
                .Distinct()
                .ToList();

            var locationMap = new Dictionary<Guid, string>();
            if (locationIds.Count > 0)
            {
                var locationQuery = await _locationRepository.GetQueryableAsync();
                var locations = await AsyncExecuter.ToListAsync(locationQuery.Where(x => locationIds.Contains(x.Id)));
                locationMap = locations.ToDictionary(x => x.Id, x => x.Code);
            }

            var dtos = reels.Select(x => MapPutawayReel(x, inventories, locationMap)).ToList();

            return new PagedResultDto<PutawayReelDto>(totalCount, dtos);
        }

        private static PutawayTaskDto MapPutawayTask(PutawayTask task)
        {
            return new PutawayTaskDto
            {
                Id = task.Id,
                TaskNo = task.TaskNo,
                ReelNo = task.ReelNo,
                FromLocationCode = task.FromLocationCode,
                ToLocationCode = task.ToLocationCode,
                Status = task.Status,
                CreationTime = task.CreationTime
            };
        }

        private static PutawayReelDto MapPutawayReel(Reel reel, List<Inventory> inventories, Dictionary<Guid, string> locationMap)
        {
            var items = inventories
                .Where(x => x.ReelId == reel.Id)
                .Select(x => new PutawayReelItemDto
                {
                    ProductCode = x.Product?.Code,
                    ProductName = x.Product?.Name,
                    BatchNo = x.BatchNo,
                    Quantity = x.Quantity,
                    Unit = x.Unit
                })
                .ToList() ?? new();

            var isMixed = items
                .Select(x => x.ProductCode)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.Ordinal)
                .Skip(1)
                .Any();

            var displayProductName = isMixed
                ? "多物料混放"
                : items.FirstOrDefault()?.ProductName;

            var displayQuantity = isMixed
                ? "见明细"
                : BuildQuantityDisplay(items);

            return new PutawayReelDto
            {
                Id = reel.Id,
                ReelNo = reel.ReelNo,
                LocationCode = reel.CurrentLocationId.HasValue && locationMap.TryGetValue(reel.CurrentLocationId.Value, out var code)
                    ? code
                    : null,
                ReelStatus = reel.Status,
                IsLocked = reel.IsLocked,
                Items = items,
                IsMixed = isMixed,
                DisplayProductName = displayProductName,
                DisplayQuantity = displayQuantity
            };
        }

        private static string BuildQuantityDisplay(List<PutawayReelItemDto> items)
        {
            if (items.Count == 0)
            {
                return string.Empty;
            }

            var totalQuantity = items.Sum(x => x.Quantity);
            var unit = items[0].Unit;

            return string.IsNullOrWhiteSpace(unit)
                ? totalQuantity.ToString()
                : $"{totalQuantity} {unit}";
        }
    }
}