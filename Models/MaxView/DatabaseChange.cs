namespace LogBook.Models.MaxView
{
    public class DatabaseChange
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string OriginalValue { get; set; }
        public string Value { get; set; }
        public int IndexPosition { get; set; }
        public int SystemDatabaseID { get; set; }
    }
}