using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LogBook.Models.AIMS
{
    public class AIMSWorkOrders
    {
        public int IntId { get; set; }
        public int ExtId { get; set; }
        //public int ActiveNoticeTypeId { get; set; }
        //public int WorkOrderStatusTypeid { get; set; }
        //public int WorkOrderPriorityTypeid { get; set; }
        public PhysicalLocation Location { get; set; }
        public string ShortDescription { get; set; }

        public ComponentType Category { get; set; }

        public WorkOrderFailure FailureType { get; set; }

        public DateTime? FaultTime { get; set; }

        public WorkGroupType WorkGroup { get; set; }

        public User AssignedUser { get; set; }

        public Component Component { get; set; }

        public WorkOrderCaseNumberType CaseNumberType { get; set; }

        public string CaseNumber { get; set; }

        public WorkOrderPriorityType Priority { get; set; }

        public IEnumerable<WorkOrderToProjectType> ProjectAssociation { get; set; }

        public IEnumerable<WorkOrderComment> Comment { get; set; }

        public IEnumerable<User> AdditionalEmails { get; set; }

        public IEnumerable<WorkOrderAttachment> Attachments { get; set; }

        public IEnumerable<WorkOrderToSubLocation> SubLocations { get; set; }

        public string SessionKey { get; set; }

        public bool IsCallbackRequested { get; set; }

        public bool IsCallBackCompleted { get; set; }

        public WorkOrderNoticeType ActiveNoticeType { get; set; }

        public WorkOrderStatusType Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? LastUpdated { get; set; }

        public User CreatedBy { get; set; }

        public User UpdatedBy { get; set; }

        public List<double?> LatLon { get; set; }  

        public DateTime? ClosedOn { get; set; }

        public IEnumerable<WorkOrderToWorkOrderNoticeType> WorkOrderNoticeTypes { get; set; }

        public string AssignedToWorkGroupOrUser { get; set; }

        public bool IsNoActionTaken { get; set; }

        public string OtherDevice { get; set; }

        public bool IsSourceOfProblemUnknown { get; set; }

        public bool ShouldSendNotifications { get; set; }
        public bool NoProblemObserved { get; set; }
        public bool IsOnHold { get; set; }

        public long? ResponseTimeTicks { get; set; }

        public long? TravelTimeTicks { get; set; }

        public long? RepairTimeTicks { get; set; }

        public long? TotalDownTimeTicks { get; set; }

        public Nullable<bool> SendEmailOnClose { get; set; }

        public Nullable<int> StatusIdBeforeHold { get; set; }

        public IEnumerable<string> TransSuiteIDs { get; set; }

        public User ReportedBy { get; set; }

        public string CurrentStatus { get; set; }

        public string SelectedComponentName { get; set; }

        public string TextTransSuiteIds { get; set; }

    }
}