using System;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.MasterData.Warehouses.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.MasterData.Warehouses
{
    public class ZoneAppService : ApplicationService, IZoneAppService
    {
        private readonly IRepository<Zone, Guid> _zoneRepository;

        public ZoneAppService(IRepository<Zone, Guid> zoneRepository)
        {
            _zoneRepository = zoneRepository;
        }

        public async Task<ZoneDto> GetAsync(Guid id)
        {
            var entity = await _zoneRepository.GetAsync(id);
            return ObjectMapper.Map<Zone, ZoneDto>(entity);
        }

        public async Task<ListResultDto<ZoneDto>> GetListAsync()
        {
            var entities = await _zoneRepository.GetListAsync();
            var items = entities.Select(ObjectMapper.Map<Zone, ZoneDto>).ToList();
            return new ListResultDto<ZoneDto>(items);
        }

        public async Task<ZoneDto> CreateAsync(CreateUpdateZoneDto input)
        {
            var entity = new Zone(
                GuidGenerator.Create(),
                input.WarehouseId,
                input.Code,
                input.Name,
                input.ZoneType);

            await _zoneRepository.InsertAsync(entity);

            return ObjectMapper.Map<Zone, ZoneDto>(entity);
        }

        public async Task<ZoneDto> UpdateAsync(Guid id, CreateUpdateZoneDto input)
        {
            var entity = await _zoneRepository.GetAsync(id);

            entity.Update(input.WarehouseId, input.Code, input.Name, input.ZoneType);

            await _zoneRepository.UpdateAsync(entity);

            return ObjectMapper.Map<Zone, ZoneDto>(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _zoneRepository.DeleteAsync(id);
        }
    }
}
