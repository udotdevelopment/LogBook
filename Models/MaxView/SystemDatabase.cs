using System;

namespace LogBook.Models.MaxView
{
    public class SystemDatabase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public int DatabaseStatusId { get; set; }
        public int? UserId { get; set; }
        //public int BinaryDatabaseId { get; set; }
        //public int BinDatabaseId { get; set; }
        //public int CompletedTables { get; set; }
        //public int TotalTables { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int SystemDatabaseTypeID { get; set; }
        //public string Version { get; set; }
        //public bool HasDatabaseChanges { get; set; }
        public Int16 DeviceId { get; set; }
        //public DateTime AcceptedDate { get; set; }
        //public string AcceptedComment { get; set; }
    }
}