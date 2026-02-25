using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.MasterData.Warehouses.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.MasterData.Warehouses
{
    public class LocationAppService : ApplicationService, ILocationAppService
    {
        private readonly IRepository<Location, Guid> _locationRepository;

        public LocationAppService(IRepository<Location, Guid> locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<LocationDto> GetAsync(Guid id)
        {
            var entity = await _locationRepository.GetAsync(id);
            return ObjectMapper.Map<Location, LocationDto>(entity);
        }

        public async Task<ListResultDto<LocationDto>> GetListAsync()
        {
            var entities = await _locationRepository.GetListAsync();
            var items = entities.Select(ObjectMapper.Map<Location, LocationDto>).ToList();
            return new ListResultDto<LocationDto>(items);
        }

        public async Task<LocationDto> CreateAsync(CreateUpdateLocationDto input)
        {
            var entity = new Location(
                GuidGenerator.Create(),
                input.ZoneId,
                input.Code,
                input.Aisle,
                input.Rack,
                input.Level,
                input.Bin,
                input.MaxWeight,
                input.MaxVolume,
                input.WarehouseId,
                input.MaxReelCount
                );

            entity.Update(
                input.ZoneId,
                input.Code,
                input.Aisle,
                input.Rack,
                input.Level,
                input.Bin,
                input.MaxWeight,
                input.MaxVolume,
                input.MaxReelCount,
                input.WarehouseId,
                input.Type,
                input.Status,
                input.AllowMixedProducts,
                input.AllowMixedBatches);

            await _locationRepository.InsertAsync(entity);

            return ObjectMapper.Map<Location, LocationDto>(entity);
        }

        public async Task<LocationDto> UpdateAsync(Guid id, CreateUpdateLocationDto input)
        {
            var entity = await _locationRepository.GetAsync(id);

            entity.Update(
                input.ZoneId,
                input.Code,
                input.Aisle,
                input.Rack,
                input.Level,
                input.Bin,
                input.MaxWeight,
                input.MaxVolume,
                input.MaxReelCount,
                input.WarehouseId,
                input.Type,
                input.Status,
                input.AllowMixedProducts,
                input.AllowMixedBatches);

            await _locationRepository.UpdateAsync(entity);

            return ObjectMapper.Map<Location, LocationDto>(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _locationRepository.DeleteAsync(id);
        }

        public async Task BatchCreateAsync(BatchCreateLocationDto input)
        {
            var locations = new List<Location>();

            for (var rack = 1; rack <= input.RackCount; rack++)
            {
                for (var level = 1; level <= input.LevelCount; level++)
                {
                    var code = $"{input.AislePrefix}-{rack:00}-{level:00}";
                    var location = new Location(
                        GuidGenerator.Create(),
                        input.ZoneId,
                        code,
                        input.AislePrefix,
                        rack.ToString("00"),
                        level.ToString("00"),
                        string.Empty,
                        0m,
                        0m,
                        input.WarehouseId,
                        (int)LocationStatus.Idle);

                    locations.Add(location);
                }
            }

            await _locationRepository.InsertManyAsync(locations, autoSave: true);
        }
    }
}
