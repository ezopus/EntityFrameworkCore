using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data
{
    public class Player
    {
        public Player()
        {
            PlayersStatistics = new List<Game>();
        }
        [Key]
        public int PlayerId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public int SquadNumber { get; set; }
        public bool IsInjured { get; set; } = false;
        public int PositionId { get; set; }
        [ForeignKey(nameof(PositionId))]
        public Position Position { get; set; }

        public int TeamId { get; set; }
        [ForeignKey(nameof(TeamId))]
        public Team Team { get; set; }

        public int TownId { get; set; }
        [ForeignKey(nameof(TownId))]
        public Town Town { get; set; }

        public virtual ICollection<Game> PlayersStatistics { get; set; }
    }
}
