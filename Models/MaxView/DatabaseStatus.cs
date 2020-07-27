using System.ComponentModel.DataAnnotations.Schema;

namespace LogBook.Models.MaxView
{
    [Table("DatabaseStatus")]
    public class DatabaseStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}