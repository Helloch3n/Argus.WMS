using System;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.MasterData.Warehouses.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.MasterData.Warehouses
{
    public class WarehouseAppService : ApplicationService, IWarehouseAppService
    {
        private readonly IRepository<Warehouse, Guid> _warehouseRepository;
        private readonly IRepository<Location, Guid> _locationRepository;

        public WarehouseAppService(
            IRepository<Warehouse, Guid> warehouseRepository,
            IRepository<Location, Guid> locationRepository)
        {
            _warehouseRepository = warehouseRepository;
            _locationRepository = locationRepository;
        }

        public async Task<WarehouseDto> GetAsync(Guid id)
        {
            var entity = await _warehouseRepository.WithDetailsAsync(x => x.Zones);
            var warehouse = entity.FirstOrDefault(x => x.Id == id);
            return ObjectMapper.Map<Warehouse, WarehouseDto>(warehouse);
        }

        public async Task<WarehouseWithDetailsDto> GetWithDetailsAsync(Guid id)
        {
            var queryable = await _warehouseRepository.WithDetailsAsync(x => x.Zones);
            var warehouse = queryable.FirstOrDefault(x => x.Id == id)
                ?? throw new Volo.Abp.AbpException($"Warehouse with id {id} not found.");

            var locationQueryable = await _locationRepository.GetQueryableAsync();
            var locations = locationQueryable.Where(l => l.WarehouseId == id).ToList();

            var locationsByZone = locations
                .GroupBy(l => l.ZoneId)
                .ToDictionary(g => g.Key, g => g.ToList());

            return new WarehouseWithDetailsDto
            {
                Id = warehouse.Id,
                Code = warehouse.Code,
                Name = warehouse.Name,
                Address = warehouse.Address,
                Manager = warehouse.Manager,
                Zones = warehouse.Zones.Select(z => new ZoneWithLocationsDto
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

        public async Task<ListResultDto<WarehouseDto>> GetListAsync()
        {
            var entities = await _warehouseRepository.WithDetailsAsync(x => x.Zones);
            var items = entities.Select(ObjectMapper.Map<Warehouse, WarehouseDto>).ToList();
            return new ListResultDto<WarehouseDto>(items);
        }

        public async Task<WarehouseDto> CreateAsync(CreateUpdateWarehouseDto input)
        {
            var entity = new Warehouse(
                GuidGenerator.Create(),
                input.Code,
                input.Name,
                input.Address,
                input.Manager);

            await _warehouseRepository.InsertAsync(entity);

            return ObjectMapper.Map<Warehouse, WarehouseDto>(entity);
        }

        public async Task<WarehouseDto> UpdateAsync(Guid id, CreateUpdateWarehouseDto input)
        {
            var entity = await _warehouseRepository.GetAsync(id);

            entity.Update(input.Code, input.Name, input.Address, input.Manager);

            await _warehouseRepository.UpdateAsync(entity);

            return ObjectMapper.Map<Warehouse, WarehouseDto>(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _warehouseRepository.DeleteAsync(id);
        }
    }
}
