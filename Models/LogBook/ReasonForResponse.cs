using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogBook.Models.LogBook
{
    public class ReasonForResponse
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [MaxLength(10)]
        public string Abbreviation { get; set; }
        [MaxLength(10)]
        public string Group { get; set; }
        [Required]
        public int DisplayOrder { get; set; }
        public virtual List<Log> Logs { get; set; }
    }
}