using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Argus.WMS.MasterData.Warehouses
{
    public class LocationManager : DomainService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IZoneRepository _zoneRepository;
        private readonly IRepository<Warehouse, Guid> _warehouseRepository;

        public LocationManager(
            ILocationRepository locationRepository,
            IZoneRepository zoneRepository,
            IRepository<Warehouse, Guid> warehouseRepository)
        {
            _locationRepository = locationRepository;
            _zoneRepository = zoneRepository;
            _warehouseRepository = warehouseRepository;
        }

        public async Task<Location> CreateAsync(
            Guid warehouseId,
            Guid zoneId,
            string code,
            string aisle,
            string rack,
            string level,
            string bin,
            decimal maxWeight,
            decimal maxVolume,
            int maxReelCount,
            LocationType type = LocationType.Storage,
            bool allowMixedProducts = true,
            bool allowMixedBatches = true)
        {
            // 1. 校验仓库是否存在
            var warehouse = await _warehouseRepository.GetAsync(warehouseId);
            if (warehouse == null)
            {
                throw new BusinessException("WMS:WarehouseNotFound").WithData("WarehouseId", warehouseId);
            }

            // 2. 校验库区是否存在，并且是否属于该仓库 (解决逻辑泄露核心点)
            var zone = await _zoneRepository.GetAsync(zoneId);
            if (zone == null)
            {
                throw new BusinessException("WMS:ZoneNotFound").WithData("ZoneId", zoneId);
            }
            if (zone.WarehouseId != warehouseId)
            {
                throw new BusinessException("WMS:ZoneNotBelongToWarehouse")
                    .WithData("ZoneId", zoneId)
                    .WithData("WarehouseId", warehouseId);
            }

            // 3. 校验库位编码在仓库内是否唯一
            var existing = await _locationRepository.GetByCodeAsync(code);
            if (existing != null && existing.WarehouseId == warehouseId)
            {
                throw new BusinessException("WMS:LocationCodeAlreadyExists")
                    .WithData("Code", code)
                    .WithData("WarehouseId", warehouseId);
            }

            // 4. 通过 internal 构造函数创建实体
            var location = new Location(
                GuidGenerator.Create(),
                warehouseId,
                zoneId,
                code,
                aisle,
                rack,
                level,
                bin,
                maxWeight,
                maxVolume,
                maxReelCount,
                type,
                allowMixedProducts,
                allowMixedBatches);

            await _locationRepository.InsertAsync(location);
            return location;
        }

        public async Task<List<Location>> BatchCreateAsync(
            Guid warehouseId,
            Guid zoneId,
            string aislePrefix,
            int rackCount,
            int levelCount)
        {
            await _warehouseRepository.GetAsync(warehouseId);

            var zone = await _zoneRepository.GetAsync(zoneId);
            if (zone.WarehouseId != warehouseId)
            {
                throw new BusinessException("WMS:ZoneNotBelongToWarehouse")
                    .WithData("ZoneId", zoneId)
                    .WithData("WarehouseId", warehouseId);
            }

            var locations = new List<Location>();
            for (var rack = 1; rack <= rackCount; rack++)
            {
                for (var level = 1; level <= levelCount; level++)
                {
                    var code = $"{aislePrefix}-{rack:00}-{level:00}";
                    var location = new Location(
                        GuidGenerator.Create(),
                        warehouseId,
                        zoneId,
                        code,
                        aislePrefix,
                        rack.ToString("00"),
                        level.ToString("00"),
                        string.Empty,
                        0m,
                        0m,
                        1);
                    locations.Add(location);
                }
            }

            await _locationRepository.InsertManyAsync(locations, autoSave: true);
            return locations;
        }
    }
}
