using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;

public class Player
{
    public Player()
    {
        PlayersStatistics = new HashSet<PlayerStatistic>();
    }
    [Key]
    public int PlayerId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public int SquadNumber { get; set; }
    public bool IsInjured { get; set; } = false;

    [ForeignKey(nameof(Position))]
    public int PositionId { get; set; }

    public virtual Position Position { get; set; } = null!;

    [ForeignKey(nameof(Team))]
    public int TeamId { get; set; }

    public virtual Team Team { get; set; } = null!;

    [ForeignKey(nameof(Town))]
    public int TownId { get; set; }

    public virtual Town Town { get; set; } = null!;

    public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; }
}

