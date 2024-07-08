namespace P02_FootballBetting.Data;

using Common;
using Microsoft.EntityFrameworkCore;
using Models;

public class FootballBettingContext : DbContext
{
    public FootballBettingContext()
    {

    }

    public FootballBettingContext(DbContextOptions options)
        : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(DbConfig.ConnectionString);
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlayerStatistic>(entity =>
        {
            entity.HasKey(ps => new { ps.GameId, ps.PlayerId });
        });

        modelBuilder.Entity<Team>(e =>
        {
            e.HasOne(t => t.PrimaryKitColor)
                .WithMany(t => t.PrimaryKitTeams)
                .HasForeignKey(t => t.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(t => t.SecondaryKitColor)
                .WithMany(t => t.SecondaryKitTeams)
                .HasForeignKey(t => t.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasOne(g => g.HomeTeam)
                .WithMany(g => g.HomeGames)
                .HasForeignKey(g => g.HomeTeamId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(g => g.AwayTeam)
                .WithMany(g => g.AwayGames)
                .HasForeignKey(g => g.AwayTeamId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasOne(p => p.Town)
                .WithMany(p => p.Players)
                .HasForeignKey(p => p.TownId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Bet> Bets { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<PlayerStatistic> PlayersStatistics { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Town> Towns { get; set; }
    public DbSet<User> Users { get; set; }

}

