using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Argus.WMS.Inbound
{
    public interface IReceiptRepository : IRepository<Receipt, Guid>
    {
        Task<Receipt?> GetWithDetailsAsync(Guid id);
    }
}