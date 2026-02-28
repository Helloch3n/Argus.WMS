using System;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.MasterData.Reels.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Argus.WMS.MasterData.Locations;

namespace Argus.WMS.MasterData.Reels
{
    public class ReelAppService :
        CrudAppService<Reel, ReelDto, Guid, ReelSearchDto, CreateUpdateReelDto>,
        IReelAppService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILocationAppService _locationAppService;
        private readonly ReelManager _reelManager;

        public ReelAppService(
            IRepository<Reel, Guid> repository,
            ILocationRepository locationRepository,
            ILocationAppService locationAppService,
            ReelManager reelManager)
            : base(repository)
        {
            _locationRepository = locationRepository;
            _locationAppService = locationAppService;
            _reelManager = reelManager;
        }

        /// <summary>
        /// 创建盘具并返回不包含库位编码的结果。
        /// </summary>
        public override async Task<ReelDto> CreateAsync(CreateUpdateReelDto input)
        {
            await CheckCreatePolicyAsync();

            // 1. 跨聚合前置校验：校验库位存在性
            if (input.CurrentLocationId.HasValue)
            {
                await _locationAppService.EnsureExistsAsync(input.CurrentLocationId.Value);
            }

            // 2. 业务规则下沉：调用领域服务创建盘具
            var id = GuidGenerator.Create();
            var entity = await _reelManager.CreateAsync(
                id,
                input.ReelNo,
                input.Name,
                input.Size,
                input.SelfWeight,
                input.CurrentLocationId);

            // 3. 持久化并返回 DTO
            await Repository.InsertAsync(entity);
            return ObjectMapper.Map<Reel, ReelDto>(entity);
        }

        /// <summary>
        /// 更新盘具当前库位。
        /// </summary>
        public async Task UpdateLocationAsync(Guid id, Guid newLocationId)
        {
            // 1. 校验目标库位
            await _locationAppService.EnsureExistsAsync(newLocationId);

            // 2. 加载并更新聚合状态
            var entity = await Repository.GetAsync(id);

            entity.SetLocation(newLocationId);

            await Repository.UpdateAsync(entity);
        }

        /// <summary>
        /// 更新盘具基础信息，并按需更新库位。
        /// </summary>
        public override async Task<ReelDto> UpdateAsync(Guid id, CreateUpdateReelDto input)
        {
            await CheckUpdatePolicyAsync();

            var entity = await Repository.GetAsync(id);

            // 1. 跨聚合前置校验：校验库位存在性
            if (input.CurrentLocationId.HasValue)
            {
                await _locationAppService.EnsureExistsAsync(input.CurrentLocationId.Value);
            }

            var reelNo = string.IsNullOrWhiteSpace(input.ReelNo)
                ? entity.ReelNo
                : input.ReelNo;

            // 2. 业务规则下沉：调用领域服务更新盘具基础信息
            await _reelManager.UpdateAsync(
                entity,
                reelNo,
                input.Name,
                input.Size,
                input.SelfWeight);

            // 3. 处理本用例下的库位变更
            if (input.CurrentLocationId.HasValue)
            {
                entity.SetLocation(input.CurrentLocationId.Value);
            }

            await Repository.UpdateAsync(entity);
            return ObjectMapper.Map<Reel, ReelDto>(entity);
        }

        /// <summary>
        /// 分页查询盘具列表并补充库位编码。
        /// </summary>
        public override async Task<PagedResultDto<ReelDto>> GetListAsync(ReelSearchDto input)
        {
            var queryable = await Repository.GetQueryableAsync();

            if (!string.IsNullOrWhiteSpace(input.ReelCode))
            {
                queryable = queryable.Where(x => x.ReelNo == input.ReelCode);
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            var items = await AsyncExecuter.ToListAsync(
                queryable
                    .OrderBy(x => x.ReelNo)
                    .PageBy(input.SkipCount, input.MaxResultCount));

            // 1. 实体映射
            var dtos = items.Select(x => ObjectMapper.Map<Reel, ReelDto>(x)).ToList();
            // 2. 补充跨聚合展示字段（库位编码）
            await PopulateLocationCodesAsync(dtos);

            return new PagedResultDto<ReelDto>(totalCount, dtos);
        }

        /// <summary>
        /// 根据主键获取盘具实体。
        /// </summary>
        protected override async Task<Reel> GetEntityByIdAsync(Guid id)
        {
            var query = await Repository.GetQueryableAsync();
            var entity = query.FirstOrDefault(x => x.Id == id);
            return entity;
        }

        /// <summary>
        /// 获取单个盘具并补充库位编码。
        /// </summary>
        public override async Task<ReelDto> GetAsync(Guid id)
        {
            await CheckGetPolicyAsync();

            // 1. 读取聚合
            var entity = await GetEntityByIdAsync(id);
            // 2. 实体映射
            var dto = ObjectMapper.Map<Reel, ReelDto>(entity);
            // 3. 通过仓储查询并补充展示字段（库位编码）
            if (dto.CurrentLocationId.HasValue)
            {
                dto.CurrentLocationCode = await _locationRepository.GetCodeByIdAsync(dto.CurrentLocationId.Value);
            }

            return dto;
        }

        /// <summary>
        /// 批量填充盘具 DTO 的库位编码。
        /// </summary>
        private async Task PopulateLocationCodesAsync(System.Collections.Generic.List<ReelDto> dtos)
        {
            var locationIds = dtos
                .Where(x => x.CurrentLocationId.HasValue)
                .Select(x => x.CurrentLocationId!.Value)
                .Distinct()
                .ToList();

            if (locationIds.Count == 0)
            {
                return;
            }

            var locationMap = await _locationRepository.GetCodeMapByIdsAsync(locationIds);

            foreach (var dto in dtos)
            {
                if (dto.CurrentLocationId.HasValue && locationMap.TryGetValue(dto.CurrentLocationId.Value, out var code))
                {
                    dto.CurrentLocationCode = code;
                }
            }
        }
    }
}