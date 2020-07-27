using System;

namespace LogBook.Models.AIMS
{
    public class WorkOrderToWorkOrderNoticeType
    {
        public Staff AssignedUser { get; set; }

        public int? UpdatedByUserId { get; set; }

        public int WorkOrderIntId { get; set; }

        public int WorkOrderNoticeTypeId { get; set; }

        public int AssigneeUserId { get; set; }

        public DateTime? DateCreated { get; set; }

        public WorkOrderNoticeType NoticeType { get; set; }

        public DateTime? LastUpdated { get; set; }
    }
}