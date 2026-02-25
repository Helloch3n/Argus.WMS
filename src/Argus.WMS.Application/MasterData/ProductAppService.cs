using System;
using System.Linq;
using System.Threading.Tasks;
using Argus.WMS.MasterData.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.MasterData
{
    public class ProductAppService :
        CrudAppService<Product, ProductDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateProductDto>,
        IProductAppService
    {
        public ProductAppService(IRepository<Product, Guid> repository)
            : base(repository)
        {
        }

        public override async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
        {
            var product = new Product(
                GuidGenerator.Create(),
                input.Code,
                input.Name,
                input.Unit,
                input.AuxUnit,
                input.ConversionRate,
                input.IsBatchManagementEnabled,
                input.ShelfLifeDays);

            await Repository.InsertAsync(product);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        public override async Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input)
        {
            var product = await Repository.GetAsync(id);

            product.SetCode(input.Code);
            product.SetName(input.Name);
            product.SetUnit(input.Unit);
            product.SetAuxUnit(input.AuxUnit);
            product.SetConversionRate(input.ConversionRate);
            product.SetBatchManagement(input.IsBatchManagementEnabled, input.ShelfLifeDays);

            await Repository.UpdateAsync(product);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        public override async Task DeleteAsync(Guid id)
        {
            await Repository.DeleteAsync(id);
        }

        public override async Task<ProductDto> GetAsync(Guid id)
        {
            var product = await Repository.GetAsync(id);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        public override async Task<PagedResultDto<ProductDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await Repository.GetQueryableAsync();

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            var items = await AsyncExecuter.ToListAsync(
                queryable
                    .OrderBy(x => x.Code)
                    .PageBy(input.SkipCount, input.MaxResultCount));

            var dtos = ObjectMapper.Map<System.Collections.Generic.List<Product>, System.Collections.Generic.List<ProductDto>>(items);

            return new PagedResultDto<ProductDto>(totalCount, dtos);
        }
    }
}