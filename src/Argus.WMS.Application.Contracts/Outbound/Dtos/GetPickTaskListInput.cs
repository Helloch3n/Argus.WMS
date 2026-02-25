using Volo.Abp.Application.Dtos;

namespace Argus.WMS.Outbound.Dtos
{
    /// <summary>
    /// 拣货任务分页查询参数
    /// </summary>
    public class GetPickTaskListInput : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 料盘编号（支持模糊查询）
        /// </summary>
        public string? ReelNo { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public PickTaskStatus? Status { get; set; }
    }
}