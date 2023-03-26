namespace VaporStore.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]        
        [Range(0.0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public int DeveloperId { get; set; }

        [ForeignKey(nameof(DeveloperId))]
        public virtual Developer Developer { get; set; } = null!;

        [Required]
        public int GenreId { get; set; }

        [ForeignKey(nameof(GenreId))]
        public virtual Genre Genre { get; set; } = null!;

        public virtual ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();

        [Required]
        public virtual ICollection<GameTag> GameTags { get; set; } = new HashSet<GameTag>();
    }
}

