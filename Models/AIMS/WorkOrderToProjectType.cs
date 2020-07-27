using System;

namespace LogBook.Models.AIMS
{
    public class WorkOrderToProjectType
    {
        public int WorkOrderIntId { get; set; }

        public ProjectAssociation ProjectType { get; set; }

        public int CreatedByUserId { get; set; }

        public DateTime DateCreated { get; set; }
    }
}