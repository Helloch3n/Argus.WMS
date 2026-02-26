using System;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.MasterData.Locations.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.MasterData.Locations
{
    public class LocationAppService : ApplicationService, ILocationAppService
    {
        private readonly IRepository<Location, Guid> _locationRepository;
        private readonly LocationManager _locationManager;

        public LocationAppService(
            IRepository<Location, Guid> locationRepository,
            LocationManager locationManager)
        {
            _locationRepository = locationRepository;
            _locationManager = locationManager;
        }

        public async Task<LocationDto> GetAsync(Guid id)
        {
            var entity = await _locationRepository.GetAsync(id);
            return ObjectMapper.Map<Location, LocationDto>(entity);
        }

        public async Task<PagedResultDto<LocationDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _locationRepository.GetCountAsync();
            var sorting = string.IsNullOrWhiteSpace(input.Sorting) ? "CreationTime DESC" : input.Sorting;
            var entities = await _locationRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, sorting);
            var items = entities.Select(ObjectMapper.Map<Location, LocationDto>).ToList();
            return new PagedResultDto<LocationDto>(totalCount, items);
        }

        public async Task<LocationDto> CreateAsync(CreateUpdateLocationDto input)
        {
            var entity = await _locationManager.CreateAsync(
                input.WarehouseId,
                input.ZoneId,
                input.Code,
                input.Aisle,
                input.Rack,
                input.Level,
                input.Bin,
                input.MaxWeight,
                input.MaxVolume,
                input.MaxReelCount,
                input.Type,
                input.AllowMixedProducts,
                input.AllowMixedBatches);

            return ObjectMapper.Map<Location, LocationDto>(entity);
        }

        public async Task<LocationDto> UpdateAsync(Guid id, CreateUpdateLocationDto input)
        {
            var entity = await _locationRepository.GetAsync(id);

            entity.UpdateBasicInfo(
                input.Code,
                input.Aisle,
                input.Rack,
                input.Level,
                input.Bin,
                input.MaxWeight,
                input.MaxVolume,
                input.MaxReelCount,
                input.Type,
                input.AllowMixedProducts,
                input.AllowMixedBatches);

            await _locationRepository.UpdateAsync(entity);

            return ObjectMapper.Map<Location, LocationDto>(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _locationRepository.DeleteAsync(id);
        }

        public async Task<ListResultDto<LocationDto>> BatchCreateAsync(BatchCreateLocationDto input)
        {
            var entities = await _locationManager.BatchCreateAsync(
                input.WarehouseId,
                input.ZoneId,
                input.AislePrefix,
                input.RackCount,
                input.LevelCount);

            var items = entities.Select(ObjectMapper.Map<Location, LocationDto>).ToList();
            return new ListResultDto<LocationDto>(items);
        }
    }
}
