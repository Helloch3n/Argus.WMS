using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Argus.WMS.Outbound.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Argus.WMS.Outbound
{
    /// <summary>
    /// 拣货任务应用服务接口
    /// </summary>
    public interface IPickTaskAppService : IApplicationService
    {
        /// <summary>
        /// 分页查询拣货任务列表
        /// </summary>
        /// <param name="input">查询参数（包含分页、排序和过滤条件）</param>
        /// <returns>分页结果</returns>
        Task<PagedResultDto<PickTaskDto>> GetListAsync(GetPickTaskListInput input);

        /// <summary>
        /// 根据出库单 ID 获取所有相关的拣货任务
        /// </summary>
        /// <param name="outboundOrderId">出库单 ID</param>
        /// <returns>拣货任务列表</returns>
        Task<List<PickTaskDto>> GetListByOutboundOrderIdAsync(Guid outboundOrderId);
    }
}