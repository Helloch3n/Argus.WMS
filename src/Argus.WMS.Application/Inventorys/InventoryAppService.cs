using System;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.Application.Contracts.Inventorys;
using Argus.WMS.Application.Contracts.Inventorys.Dtos;
using Argus.WMS.Application.Mappers;
using Argus.WMS.Inventorys;
using Argus.WMS.MasterData.Reels;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.Application.Inventorys
{
    public class InventoryAppService : ApplicationService, IInventoryAppService
    {
        private readonly IRepository<Inventory, Guid> _inventoryRepository;
        private readonly InventoryApplicationMappers _inventoryMapper;
        private readonly InventoryManager _inventoryManager;
        private readonly IRepository<Reel, Guid> _reelRepository;

        public InventoryAppService(
            IRepository<Inventory, Guid> inventoryRepository,
            InventoryApplicationMappers inventoryMapper,
            InventoryManager inventoryManager,
            IRepository<Reel, Guid> reelRepository)
        {
            _inventoryRepository = inventoryRepository;
            _inventoryMapper = inventoryMapper;
            _inventoryManager = inventoryManager;
            _reelRepository = reelRepository;
        }

        public async Task<PagedResultDto<InventoryDto>> GetListAsync(InventorySearchDto input)
        {
            var query = await _inventoryRepository.WithDetailsAsync(x => x.Reel,
                x => x.Reel.CurrentLocation, x => x.Product);

            if (!string.IsNullOrWhiteSpace(input.ReelNo))
            {
                query = query.Where(x => x.Reel.ReelNo.Contains(input.ReelNo));
            }

            if (input.ProductId.HasValue)
            {
                query = query.Where(x => x.ProductId == input.ProductId.Value);
            }

            if (!string.IsNullOrWhiteSpace(input.Source_WO))
            {
                query = query.Where(x => x.Source_WO.Contains(input.Source_WO));
            }

            var totalCount = await AsyncExecuter.CountAsync(query);

            query = query
                .OrderBy(x => x.Reel.ReelNo)
                .ThenByDescending(x => x.Index)
                .PageBy(input.SkipCount, input.MaxResultCount);

            var items = await AsyncExecuter.ToListAsync(query);

            var result = items.Select(_inventoryMapper.Map).ToList();
            return new PagedResultDto<InventoryDto>(totalCount, result);
        }

        //public async Task<InventoryDto> ProductionReceiveAsync(ProductionReceiveInput input)
        //{
        //    var inventory = await _inventoryManager.ProductionReceiveAsync(
        //        input.ReelId,
        //        input.ProductId,
        //        input.Quantity,
        //        input.Weight,
        //        input.BatchNo,
        //        input.Source_WO,
        //        input.LocationId,
        //        input.SN,
        //        input.CraftVersion,
        //        input.Unit);

        //    // 重新查询以加载导航属性
        //    var query = await _inventoryRepository.WithDetailsAsync(
        //        x => x.Reel, x => x.Reel.CurrentLocation, x => x.Product);

        //    var entity = await AsyncExecuter.FirstOrDefaultAsync(
        //        query.Where(x => x.Id == inventory.Id));

        //    return _inventoryMapper.Map(entity);
        //}
    }
}