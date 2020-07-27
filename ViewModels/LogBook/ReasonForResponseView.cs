using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogBook.ViewModels.LogBook
{
    public class ReasonForResponseView
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        public string Group { get; set; }

        public string Abbreviation { get; set; }
        [Required]
        public int DisplayOrder { get; set; }
        public virtual List<LogView> Logs { get; set; }
    }
}