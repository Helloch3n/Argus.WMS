using System;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.MasterData.Locations;
using Argus.WMS.MasterData.Warehouses;
using Argus.WMS.MasterData.Zones.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.MasterData.Zones
{
    public class ZoneAppService : ApplicationService, IZoneAppService
    {
        private readonly IRepository<Zone, Guid> _zoneRepository;
        private readonly IRepository<Warehouse, Guid> _warehouseRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ZoneManager _zoneManager;

        public ZoneAppService(
            IRepository<Zone, Guid> zoneRepository,
            IRepository<Warehouse, Guid> warehouseRepository,
            ILocationRepository locationRepository,
            ZoneManager zoneManager)
        {
            _zoneRepository = zoneRepository;
            _warehouseRepository = warehouseRepository;
            _locationRepository = locationRepository;
            _zoneManager = zoneManager;
        }

        public async Task<ZoneDto> GetAsync(Guid id)
        {
            var entity = await _zoneRepository.GetAsync(id);
            return ObjectMapper.Map<Zone, ZoneDto>(entity);
        }

        public async Task<PagedResultDto<ZoneDto>> GetListAsync(ZoneSearchDto input)
        {
            var query = await _zoneRepository.GetQueryableAsync();

            if (!string.IsNullOrWhiteSpace(input.ZoneCode))
            {
                query = query.Where(x => x.Code.Contains(input.ZoneCode));
            }

            if (!string.IsNullOrWhiteSpace(input.ZoneName))
            {
                query = query.Where(x => x.Name.Contains(input.ZoneName));
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
            var items = entities.Select(ObjectMapper.Map<Zone, ZoneDto>).ToList();
            return new PagedResultDto<ZoneDto>(totalCount, items);
        }

        public async Task<ZoneDto> CreateAsync(CreateUpdateZoneDto input)
        {
            var entity = await _zoneManager.CreateAsync(
                input.WarehouseId,
                input.Code,
                input.Name,
                input.ZoneType);

            return ObjectMapper.Map<Zone, ZoneDto>(entity);
        }

        public async Task<ZoneDto> UpdateAsync(Guid id, CreateUpdateZoneDto input)
        {
            var entity = await _zoneRepository.GetAsync(id);

            entity.Update(input.Code, input.Name, input.ZoneType);

            await _zoneRepository.UpdateAsync(entity);

            return ObjectMapper.Map<Zone, ZoneDto>(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var hasLocations = await _locationRepository.AnyAsync(x => x.ZoneId == id);

            if (hasLocations)
            {
                throw new BusinessException("Zone:HasLocations")
                    .WithData("ZoneId", id);
            }

            await _zoneRepository.DeleteAsync(id);
        }
    }
}
