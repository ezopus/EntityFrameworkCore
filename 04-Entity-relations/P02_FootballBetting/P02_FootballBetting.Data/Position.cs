using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data
{
    public class Position
    {
        public Position()
        {
            Players = new List<Player>();
        }
        [Key]
        public int PositionId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public virtual ICollection<Player> Players { get; set; }
    }
}
