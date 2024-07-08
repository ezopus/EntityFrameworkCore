using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data
{
    public class Game
    {
        public Game()
        {
            HomeTeams = new List<Team>();
            AwayTeams = new List<Team>();
            PlayersStatistics = new List<Player>();
            Bets = new List<Bet>();
        }
        [Key]
        public int GameId { get; set; }
        [Required]
        public int HomeTeamId { get; set; }
        [ForeignKey(nameof(HomeTeamId))]
        public Team HomeTeam { get; set; }

        [Required]
        public int AwayTeamId { get; set; }
        [ForeignKey(nameof(AwayTeamId))]
        public Team AwayTeam { get; set; }

        [Required]
        public int HomeTeamGoals { get; set; }
        [Required]
        public int AwayTeamGoals { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public decimal HomeTeamBetRate { get; set; }
        [Required]
        public decimal AwayTeamBetRate { get; set; }
        [Required]
        public decimal DrawBetRate { get; set; }
        [Required]
        public string Result { get; set; }
        public virtual ICollection<Team> HomeTeams { get; set; }
        public virtual ICollection<Team> AwayTeams { get; set; }
        public virtual ICollection<Player> PlayersStatistics { get; set; }
        public virtual ICollection<Bet> Bets { get; set; }
    }
}
