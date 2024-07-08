using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data
{
    public class Town
    {
        public Town()
        {
            Teams = new List<Team>();
        }
        [Key]
        public int TownId { get; set; }

        [Required]
        public string Name { get; set; }

        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }

        public ICollection<Team> Teams { get; set; }
    }
}
