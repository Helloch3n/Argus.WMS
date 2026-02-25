using Argus.WMS.Application.Contracts.Inbound.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Argus.WMS.Inbound
{
    public interface IReceiptAppService : IApplicationService
    {
        Task<ReceiptDto> GetAsync(Guid id);
        Task<PagedResultDto<ReceiptDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<ReceiptDto> CreateAsync(ReceiptCreateDto input);
        Task ExecuteAsync(ExecuteReceiptInput input);
        Task ExecuteByReelAsync(ExecuteReceiptByReelInput input);
    }
}