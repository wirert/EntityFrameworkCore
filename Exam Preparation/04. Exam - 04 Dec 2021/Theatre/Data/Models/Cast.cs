namespace Theatre.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Cast
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string FullName { get; set; } = null!;

        public bool IsMainCharacter { get; set; }

        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; } = null!;

        public int PlayId { get; set; }

        [ForeignKey(nameof(PlayId))]
        public virtual Play Play { get; set; } = null!;
    }
}
