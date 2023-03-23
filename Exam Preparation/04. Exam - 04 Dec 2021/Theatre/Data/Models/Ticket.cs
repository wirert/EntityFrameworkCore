namespace Theatre.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public sbyte RowNumber { get; set; }

        public int PlayId { get; set; }

        [ForeignKey(nameof(PlayId))]
        public virtual Play Play { get; set; } = null!;

        public int TheatreId { get; set; }

        [ForeignKey(nameof(TheatreId))]
        public virtual Theatre Theatre { get; set; } = null!;
    }
}
