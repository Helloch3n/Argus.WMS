using System;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.MasterData.Reels.Dtos;
using Argus.WMS.Mappers;
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
        private readonly IRepository<Location, Guid> _locationRepository;
        private readonly ReelApplicationMappers _reelMapper;

        public ReelAppService(
            IRepository<Reel, Guid> repository,
            IRepository<Location, Guid> locationRepository,
            ReelApplicationMappers reelMapper)
            : base(repository)
        {
            _locationRepository = locationRepository;
            _reelMapper = reelMapper;
        }

        public override async Task<ReelDto> CreateAsync(CreateUpdateReelDto input)
        {
            await CheckCreatePolicyAsync();

            var reelNo = string.IsNullOrWhiteSpace(input.ReelNo)
                ? await GenerateReelNoAsync()
                : input.ReelNo;

            await EnsureReelNoUniqueAsync(reelNo);

            if (input.CurrentLocationId.HasValue)
            {
                await EnsureLocationExistsAsync(input.CurrentLocationId.Value);
            }

            var entity = new Reel(
                GuidGenerator.Create(),
                reelNo,
                input.Name,
                input.Type,
                input.SelfWeight,
                input.MaxWeight,
                ReelStatus.Empty,
                input.CurrentLocationId);

            await Repository.InsertAsync(entity, autoSave: true);

            var query = await Repository.WithDetailsAsync(x => x.CurrentLocation);
            var createdEntity = query.First(x => x.Id == entity.Id);

            return _reelMapper.Map(createdEntity);
        }

        public async Task UpdateLocationAsync(Guid id, Guid newLocationId)
        {
            await EnsureLocationExistsAsync(newLocationId);

            var entity = await Repository.GetAsync(id);

            entity.SetLocation(newLocationId);

            await Repository.UpdateAsync(entity, autoSave: true);
        }

        public override async Task<ReelDto> UpdateAsync(Guid id, CreateUpdateReelDto input)
        {
            await CheckUpdatePolicyAsync();

            var entity = await Repository.GetAsync(id);

            if (!string.IsNullOrWhiteSpace(input.ReelNo) && !string.Equals(entity.ReelNo, input.ReelNo, StringComparison.Ordinal))
            {
                await EnsureReelNoUniqueAsync(input.ReelNo);
            }

            if (input.CurrentLocationId.HasValue)
            {
                await EnsureLocationExistsAsync(input.CurrentLocationId.Value);
            }

            var reelNo = string.IsNullOrWhiteSpace(input.ReelNo)
                ? entity.ReelNo
                : input.ReelNo;

            entity.Update(
                reelNo,
                input.Name,
                input.Type,
                input.SelfWeight,
                input.MaxWeight);

            if (input.CurrentLocationId.HasValue)
            {
                entity.SetLocation(input.CurrentLocationId.Value);
            }

            await Repository.UpdateAsync(entity, autoSave: true);

            var query = await Repository.WithDetailsAsync(x => x.CurrentLocation);
            var updatedEntity = query.First(x => x.Id == entity.Id);

            return _reelMapper.Map(updatedEntity);
        }

        public override async Task<PagedResultDto<ReelDto>> GetListAsync(ReelSearchDto input)
        {
            var queryable = await Repository.WithDetailsAsync(x => x.CurrentLocation);

            if (!string.IsNullOrWhiteSpace(input.ReelCode))
            {
                queryable = queryable.Where(x => x.ReelNo == input.ReelCode);
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            var items = await AsyncExecuter.ToListAsync(
                queryable
                    .OrderBy(x => x.ReelNo)
                    .PageBy(input.SkipCount, input.MaxResultCount));

            var dtos = items.Select(x => _reelMapper.Map(x)).ToList();

            return new PagedResultDto<ReelDto>(totalCount, dtos);
        }

        protected override async Task<Reel> GetEntityByIdAsync(Guid id)
        {
            var query = await Repository.WithDetailsAsync(x => x.CurrentLocation);
            var entity = query.FirstOrDefault(x => x.Id == id);
            return entity;
        }

        protected override ReelDto MapToGetOutputDto(Reel entity)
        {
            return _reelMapper.Map(entity);
        }

        protected override ReelDto MapToGetListOutputDto(Reel entity)
        {
            return _reelMapper.Map(entity);
        }

        private async Task EnsureLocationExistsAsync(Guid locationId)
        {
            var exists = await _locationRepository.AnyAsync(x => x.Id == locationId);

            if (!exists)
            {
                throw new BusinessException("Reel:LocationNotFound")
                    .WithData("LocationId", locationId);
            }
        }

        private async Task EnsureReelNoUniqueAsync(string reelNo)
        {
            var exists = await Repository.AnyAsync(x => x.ReelNo == reelNo);

            if (exists)
            {
                throw new BusinessException("Reel:ReelNoAlreadyExists")
                    .WithData("ReelNo", reelNo);
            }
        }

        private async Task<string> GenerateReelNoAsync()
        {
            var date = Clock.Now.ToString("yyyyMMdd");
            for (var i = 0; i < 100; i++)
            {
                var reelNo = $"R-{date}-{Random.Shared.Next(1, 1000):D3}";
                var exists = await Repository.AnyAsync(x => x.ReelNo == reelNo);

                if (!exists)
                {
                    return reelNo;
                }
            }

            throw new BusinessException("Reel:GenerateReelNoFailed");
        }
    }
}