using System;

namespace LogBook.Models.AIMS
{
    public class WorkOrderComment
    {
        public int IntId { get; set; }

        public int WorkOrderIntId { get; set; }

        public string Comments { get; set; }

        public User CreatedByUser { get; set; }

        public User UpdatedByUser { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? LastUpdated { get; set; }
    }
}