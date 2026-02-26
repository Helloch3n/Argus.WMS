using System;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.MasterData.Locations.Dtos;
using Argus.WMS.MasterData.Warehouses;
using Argus.WMS.MasterData.Zones;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.MasterData.Locations
{
    public class LocationAppService : ApplicationService, ILocationAppService
    {
        private readonly IRepository<Location, Guid> _locationRepository;
        private readonly IRepository<Zone, Guid> _zoneRepository;
        private readonly IRepository<Warehouse, Guid> _warehouseRepository;
        private readonly LocationManager _locationManager;

        public LocationAppService(
            IRepository<Location, Guid> locationRepository,
            IRepository<Zone, Guid> zoneRepository,
            IRepository<Warehouse, Guid> warehouseRepository,
            LocationManager locationManager)
        {
            _locationRepository = locationRepository;
            _zoneRepository = zoneRepository;
            _warehouseRepository = warehouseRepository;
            _locationManager = locationManager;
        }

        public async Task<LocationDto> GetAsync(Guid id)
        {
            var entity = await _locationRepository.GetAsync(id);
            return ObjectMapper.Map<Location, LocationDto>(entity);
        }

        public async Task<PagedResultDto<LocationDto>> GetListAsync(LocationSearchDto input)
        {
            var query = await _locationRepository.GetQueryableAsync();

            if (!string.IsNullOrWhiteSpace(input.LocationCode))
            {
                query = query.Where(x => x.Code.Contains(input.LocationCode));
            }

            if (!string.IsNullOrWhiteSpace(input.ZoneCode) || !string.IsNullOrWhiteSpace(input.ZoneName))
            {
                var zoneQuery = await _zoneRepository.GetQueryableAsync();

                if (!string.IsNullOrWhiteSpace(input.ZoneCode))
                {
                    zoneQuery = zoneQuery.Where(x => x.Code.Contains(input.ZoneCode));
                }

                if (!string.IsNullOrWhiteSpace(input.ZoneName))
                {
                    zoneQuery = zoneQuery.Where(x => x.Name.Contains(input.ZoneName));
                }

                var zoneIds = zoneQuery.Select(x => x.Id);
                query = query.Where(x => zoneIds.Contains(x.ZoneId));
            }

            if (!string.IsNullOrWhiteSpace(input.WarehouseCode) || !string.IsNullOrWhiteSpace(input.WarehouseName))
            {
                var warehouseQuery = await _warehouseRepository.GetQueryableAsync();

                if (!string.IsNullOrWhiteSpace(input.WarehouseCode))
                {
                    warehouseQuery = warehouseQuery.Where(x => x.Code.Contains(input.WarehouseCode));
                }

                if (!string.IsNullOrWhiteSpace(input.WarehouseName))
                {
                    warehouseQuery = warehouseQuery.Where(x => x.Name.Contains(input.WarehouseName));
                }

                var warehouseIds = warehouseQuery.Select(x => x.Id);
                query = query.Where(x => warehouseIds.Contains(x.WarehouseId));
            }

            var totalCount = await AsyncExecuter.CountAsync(query);

            query = query
                .OrderBy(x => x.Code)
                .PageBy(input.SkipCount, input.MaxResultCount);

            var entities = await AsyncExecuter.ToListAsync(query);
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
