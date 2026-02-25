using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Argus.WMS.Putaway
{
    public class PutawayTask : FullAuditedAggregateRoot<Guid>
    {
        public string TaskNo { get; private set; }
        public string ReelNo { get; private set; }
        public string FromLocationCode { get; private set; }
        public string? ToLocationCode { get; private set; }
        public PutawayTaskStatus Status { get; private set; }
        public Guid? AssignedUserId { get; private set; }

        protected PutawayTask()
        {
        }

        public PutawayTask(
            Guid id,
            string taskNo,
            string reelNo,
            string fromLocationCode,
            string? toLocationCode,
            Guid? assignedUserId) : base(id)
        {
            TaskNo = taskNo;
            ReelNo = reelNo;
            FromLocationCode = fromLocationCode;
            ToLocationCode = toLocationCode;
            AssignedUserId = assignedUserId;
            Status = PutawayTaskStatus.Pending;
        }

        public void Start()
        {
            Status = PutawayTaskStatus.InProgress;
        }

        public void Complete()
        {
            Status = PutawayTaskStatus.Completed;
        }

        public void Cancel()
        {
            Status = PutawayTaskStatus.Cancelled;
        }
    }
}