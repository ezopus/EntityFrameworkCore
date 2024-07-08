using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data
{
    public class Country
    {
        public Country()
        {
            Towns = new HashSet<Town>();
        }
        [Key]
        public int CountryId { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Town> Towns { get; set; }
    }
}
