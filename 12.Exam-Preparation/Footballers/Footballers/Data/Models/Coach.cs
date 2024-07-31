using Footballers.Shared;
using System.ComponentModel.DataAnnotations;

namespace Footballers.Data.Models
{
    public class Coach
    {
        public Coach()
        {
            Footballers = new HashSet<Footballer>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.CoachNameMaxLength, MinimumLength = GlobalConstants.CoachNameMinLength)]
        [MaxLength(GlobalConstants.CoachNameMaxLength)]
        public string Name { get; set; }

        public string Nationality { get; set; }

        public virtual ICollection<Footballer> Footballers { get; set; }

    }
}
