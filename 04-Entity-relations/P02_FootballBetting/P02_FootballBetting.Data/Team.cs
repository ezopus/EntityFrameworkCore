using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data
{
    public class Team
    {
        public Team()
        {
            Players = new List<Player>();
            HomeGames = new List<Game>();
            AwayGames = new List<Game>();
        }
        [Key]
        public int TeamId { get; set; }

        [Required]
        public string Name { get; set; }

        public string LogoUrl { get; set; }
        public string Initials { get; set; }
        public decimal Budget { get; set; }
        public int PrimaryKitColorId { get; set; }
        [ForeignKey(nameof(PrimaryKitColorId))]

        public virtual Color PrimaryColor { get; set; }

        public int SecondaryKitColorId { get; set; }
        [ForeignKey(nameof(SecondaryKitColorId))]
        public virtual Color SecondaryColor { get; set; }


        public int TownId { get; set; }
        [ForeignKey(nameof(TownId))]
        public Town Town { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public virtual ICollection<Game> HomeGames { get; set; }
        public virtual ICollection<Game> AwayGames { get; set; }

    }
}
