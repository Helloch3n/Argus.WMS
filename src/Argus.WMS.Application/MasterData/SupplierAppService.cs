using System;
using System.Threading.Tasks;
using System.Linq;
using Argus.WMS.MasterData.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.MasterData
{
    public class SupplierAppService :
        CrudAppService<
            Supplier,
            SupplierDto,
            Guid,
            SupplierSearchDto,
            CreateUpdateSupplierDto>,
        ISupplierAppService
    {
        public SupplierAppService(IRepository<Supplier, Guid> repository)
            : base(repository)
        {
        }

        public override async Task<SupplierDto> CreateAsync(CreateUpdateSupplierDto input)
        {
            var supplier = new Supplier(
                GuidGenerator.Create(),
                input.Code,
                input.Name,
                input.ContactPerson,
                input.Mobile,
                input.Email,
                input.Address
            );

            await Repository.InsertAsync(supplier);

            return ObjectMapper.Map<Supplier, SupplierDto>(supplier);
        }

        public override async Task<SupplierDto> UpdateAsync(Guid id, CreateUpdateSupplierDto input)
        {
            var supplier = await Repository.GetAsync(id);

            supplier.SetCode(input.Code);
            supplier.SetName(input.Name);
            supplier.UpdateContact(input.ContactPerson, input.Mobile, input.Email, input.Address);

            await Repository.UpdateAsync(supplier);

            return ObjectMapper.Map<Supplier, SupplierDto>(supplier);
        }
        public override async Task<PagedResultDto<SupplierDto>> GetListAsync(SupplierSearchDto input)
        {
            var queryable = await Repository.GetQueryableAsync();

            if (!string.IsNullOrWhiteSpace(input.SupplierCode))
            {
                queryable = queryable.Where(x => x.Code == input.SupplierCode);
            }

            if (!string.IsNullOrWhiteSpace(input.SupplierName))
            {
                queryable = queryable.Where(x => x.Name.Contains(input.SupplierName));
            }

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            var items = await AsyncExecuter.ToListAsync(
                queryable
                    .OrderBy(x => x.Code)
                    .PageBy(input.SkipCount, input.MaxResultCount));

            var dtos = ObjectMapper.Map<System.Collections.Generic.List<Supplier>, System.Collections.Generic.List<SupplierDto>>(items);

            return new PagedResultDto<SupplierDto>(totalCount, dtos);
        }
    }
}