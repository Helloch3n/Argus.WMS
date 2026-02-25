using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Argus.WMS.Outbound.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Argus.WMS.Outbound
{
    public interface IOutboundAppService :
        ICrudAppService<OutboundOrderDto, Guid, PagedAndSortedResultRequestDto, CreateOutboundOrderInput>
    {
        /// <summary>
        /// 完成拣货任务并同步处理库存与单据状态。
        /// </summary>
        /// <param name="taskId">拣货任务ID。</param>
        Task CompletePickTaskAsync(Guid taskId);

        Task<List<PickTaskDto>> GeneratePickTaskAsync(Guid orderId);
    }
}