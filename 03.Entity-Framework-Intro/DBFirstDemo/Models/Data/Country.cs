using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBFirstDemo.Models.Data
{
    [Table("Countries")]
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(true)]
        public string Name { get; set; } = null!;

    }
}
