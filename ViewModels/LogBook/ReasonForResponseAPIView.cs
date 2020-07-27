using System.ComponentModel.DataAnnotations;

namespace LogBook.ViewModels.LogBook
{
    public class ReasonForResponseAPIView
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }

        public string Abbreviation { get; set; }
        [Required]
        public int DisplayOrder { get; set; }
    }
}