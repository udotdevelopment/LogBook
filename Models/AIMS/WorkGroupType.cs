using System;

namespace LogBook.Models.AIMS
{
    public class WorkGroupType
    {
        public int IntId { get; set; }

        public int ExtId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsGlobal { get; set; }

        public bool IsAccessRestricted { get; set; }

        public bool IsActive { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? LastUpdated { get; set; }
    }
}