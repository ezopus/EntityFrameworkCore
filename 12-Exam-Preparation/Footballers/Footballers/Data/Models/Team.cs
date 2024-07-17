using Footballers.Shared;
using System.ComponentModel.DataAnnotations;

namespace Footballers.Data.Models
{
    public class Team
    {
        public Team()
        {
            TeamsFootballers = new HashSet<TeamFootballer>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.TeamNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(GlobalConstants.CoachNameMaxLength, MinimumLength = GlobalConstants.CoachNameMinLength)]
        [MaxLength(GlobalConstants.TeamNationalityMaxLength)]

        public string Nationality { get; set; }

        [Required]
        public int Trophies { get; set; }

        public virtual ICollection<TeamFootballer> TeamsFootballers { get; set; }


    }
}
