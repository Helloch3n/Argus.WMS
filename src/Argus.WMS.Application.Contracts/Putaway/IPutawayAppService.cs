using System;
using System.Threading.Tasks;
using Argus.WMS.Putaway.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Argus.WMS.Putaway
{
    public interface IPutawayAppService : IApplicationService
    {
        Task<PutawayTaskDto> CreateAsync(CreatePutawayTaskInput input);
        Task CompleteAsync(Guid id, CompletePutawayTaskInput input);
        Task<PagedResultDto<PutawayTaskDto>> GetListAsync(GetPutawayTaskListInput input);
        Task<PagedResultDto<PutawayReelDto>> GetAvailableReelsAsync(GetAvailableReelsInput input);
    }
}