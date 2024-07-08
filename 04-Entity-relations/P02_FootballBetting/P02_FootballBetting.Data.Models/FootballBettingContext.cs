using Microsoft.EntityFrameworkCore;

namespace P02_FootballBetting.Data.Models
{
    public class FootballBettingContext : DbContext
    {
        private const string ConnectionString =
            "Server=.;Database=FootballBookmakerSystem;TrustServerCertificate=True;Trusted Connection=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Game> Games { get; set; }

    }
}
