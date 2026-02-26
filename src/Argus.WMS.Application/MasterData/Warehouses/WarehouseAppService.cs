using System;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.MasterData.Locations;
using Argus.WMS.MasterData.Locations.Dtos;
using Argus.WMS.MasterData.Warehouses.Dtos;
using Argus.WMS.MasterData.Zones;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.MasterData.Warehouses
{
    public class WarehouseAppService : ApplicationService, IWarehouseAppService
    {
        private readonly IRepository<Warehouse, Guid> _warehouseRepository;
        private readonly IZoneRepository _zoneRepository;
        private readonly ILocationRepository _locationRepository;

        public WarehouseAppService(
            IRepository<Warehouse, Guid> warehouseRepository,
            IZoneRepository zoneRepository,
            ILocationRepository locationRepository)
        {
            _warehouseRepository = warehouseRepository;
            _zoneRepository = zoneRepository;
            _locationRepository = locationRepository;
        }

        public async Task<WarehouseDto> GetAsync(Guid id)
        {
            var warehouse = await _warehouseRepository.GetAsync(id);
            return ObjectMapper.Map<Warehouse, WarehouseDto>(warehouse);
        }

        public async Task<WarehouseWithDetailsDto> GetWithDetailsAsync(Guid id)
        {
            var warehouse = await _warehouseRepository.GetAsync(id);
            var zones = await _zoneRepository.GetListByWarehouseIdAsync(id);
            var locations = await _locationRepository.GetListByWarehouseIdAsync(id);

            var locationsByZone = locations
                .GroupBy(l => l.ZoneId)
                .ToDictionary(g => g.Key, g => g.ToList());

            return new WarehouseWithDetailsDto
            {
                Id = warehouse.Id,
                Code = warehouse.Code,
                Name = warehouse.Name,
                Zones = zones.Select(z => new ZoneWithLocationsDto
                {
                    Id = z.Id,
                    Code = z.Code,
                    Name = z.Name,
                    Locations = locationsByZone.TryGetValue(z.Id, out var zoneLocations)
                        ? zoneLocations.Select(ObjectMapper.Map<Location, LocationDto>).ToList()
                        : []
                }).ToList()
            };
        }

        public async Task<PagedResultDto<WarehouseDto>> GetListAsync(WarehouseSearchDto input)
        {
            var query = await _warehouseRepository.GetQueryableAsync();

            if (!string.IsNullOrWhiteSpace(input.WarehouseCode))
            {
                query = query.Where(x => x.Code.Contains(input.WarehouseCode));
            }

            if (!string.IsNullOrWhiteSpace(input.WarehouseName))
            {
                query = query.Where(x => x.Name.Contains(input.WarehouseName));
            }

            var totalCount = await AsyncExecuter.CountAsync(query);

            query = query
                .OrderBy(x => x.Code)
                .PageBy(input.SkipCount, input.MaxResultCount);

            var entities = await AsyncExecuter.ToListAsync(query);
            var items = entities.Select(ObjectMapper.Map<Warehouse, WarehouseDto>).ToList();
            return new PagedResultDto<WarehouseDto>(totalCount, items);
        }

        public async Task<WarehouseDto> CreateAsync(CreateUpdateWarehouseDto input)
        {
                var entity = new Warehouse(
                GuidGenerator.Create(),
                input.Code,
                input.Name);

            await _warehouseRepository.InsertAsync(entity);

            return ObjectMapper.Map<Warehouse, WarehouseDto>(entity);
        }

        public async Task<WarehouseDto> UpdateAsync(Guid id, CreateUpdateWarehouseDto input)
        {
            var entity = await _warehouseRepository.GetAsync(id);

            entity.Update(input.Code, input.Name);

            await _warehouseRepository.UpdateAsync(entity);

            return ObjectMapper.Map<Warehouse, WarehouseDto>(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var hasZones = await _zoneRepository.AnyAsync(x => x.WarehouseId == id);

            if (hasZones)
            {
                throw new BusinessException("Warehouse:HasZones")
                    .WithData("WarehouseId", id);
            }

            await _warehouseRepository.DeleteAsync(id);
        }
    }
}
