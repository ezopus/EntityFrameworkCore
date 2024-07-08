using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;

public class Team
{
    public Team()
    {
        Players = new HashSet<Player>();
        HomeGames = new HashSet<Game>();
        AwayGames = new HashSet<Game>();
    }
    [Key]
    public int TeamId { get; set; }

    [Required]
    public string Name { get; set; }

    public string LogoUrl { get; set; }
    public string Initials { get; set; }
    public decimal Budget { get; set; }

    [ForeignKey(nameof(PrimaryKitColor))]
    public int PrimaryKitColorId { get; set; }
    public virtual Color PrimaryKitColor { get; set; }

    [ForeignKey(nameof(SecondaryKitColor))]
    public int SecondaryKitColorId { get; set; }
    public virtual Color SecondaryKitColor { get; set; }


    [ForeignKey(nameof(Town))]
    public int TownId { get; set; }
    public Town Town { get; set; }

    public virtual ICollection<Player> Players { get; set; }

    [InverseProperty(nameof(Game.HomeTeam))]
    public virtual ICollection<Game> HomeGames { get; set; }

    [InverseProperty(nameof(Game.AwayTeam))]
    public virtual ICollection<Game> AwayGames { get; set; }

}

