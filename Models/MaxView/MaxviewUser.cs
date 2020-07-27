using System.ComponentModel.DataAnnotations.Schema;

namespace LogBook.Models.MaxView
{
    [Table("Users")]
    public class MaxviewUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }
}