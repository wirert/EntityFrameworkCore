namespace SoftJail.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Mail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [MaxLength(30)]
        public string Sender { get; set; }
        
        [Required]
        [MaxLength(60)]
        public string Address { get; set; }

        [Required]
        public int PrisonerId { get; set; }

        [ForeignKey(nameof(PrisonerId))]
        public virtual Prisoner Prisoner { get; set; } = null!;
    }
}
