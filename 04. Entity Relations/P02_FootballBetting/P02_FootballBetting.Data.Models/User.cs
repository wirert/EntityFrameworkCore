using P02_FootballBetting.Data.Common;
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
    [MaxLength(Constants.UserUsernameMaxLength)]
    public string Username { get; set; } = null!;

    [Required]
    [MaxLength(Constants.PasswordMaxLength)]
    public string Password { get; set; } = null!;

    [EmailAddress]
    [MaxLength(Constants.UserEmailMaxLength)]
    public string? Email { get; set; }

    [MaxLength(Constants.UserNameMaxLength)]
    public string? Name { get; set; }

    [Required]
    public decimal Balance { get; set; }

    public virtual ICollection<Bet> Bets { get; set; }
}
