namespace LogBook.Models.AIMS
{
    public class WorkOrderToSubLocation
    {
        public int WorkOrderIntId { get; set; }
        public bool IsRepaired { get; set; }
        public PhysicalLocation Location { get; set; }
    }
}