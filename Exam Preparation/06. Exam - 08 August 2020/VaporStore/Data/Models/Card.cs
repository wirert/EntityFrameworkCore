namespace VaporStore.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using VaporStore.Data.Models.Enums;

    public class Card
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(19)]
        public string Number { get; set; } = null!;

        [Required]
        [StringLength(3, MinimumLength = 3)]
        [RegularExpression(@"[/d]{3}")]
        public string Cvc { get; set; } = null!;

        [Required]
        public CardType Type { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        public virtual ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();
    }
}

