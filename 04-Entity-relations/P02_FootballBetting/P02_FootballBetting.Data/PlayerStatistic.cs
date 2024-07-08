using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data
{
    public class PlayerStatistic
    {
        public int GameId { get; set; }
        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; }

        public int PlayerId { get; set; }
        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; }

        public int ScoredGoals { get; set; } = 0;
        public int Assists { get; set; } = 0;
        public int MinutesPlayed { get; set; } = 0;

    }
}
