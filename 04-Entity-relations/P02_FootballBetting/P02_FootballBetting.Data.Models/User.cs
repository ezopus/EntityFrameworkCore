using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models;

public class User
{
    public User()
    {
        Bets = new HashSet<Bet>();
    }
    [Key]
    public int UserId { get; set; }

    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
    public string Name { get; set; }

    [Required]
    public string Email { get; set; } = null!;

    public decimal Balance { get; set; } = 0;

    public virtual ICollection<Bet> Bets { get; set; }

}

