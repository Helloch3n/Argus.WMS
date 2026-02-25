using System;
using System.Threading.Tasks;
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
            PagedAndSortedResultRequestDto,
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
    }
}