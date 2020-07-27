namespace LogBook.Models.AIMS
{
    public class WorkOrderFailure
    {
        public int Id { get; set; }

        public string Abbreviation { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}