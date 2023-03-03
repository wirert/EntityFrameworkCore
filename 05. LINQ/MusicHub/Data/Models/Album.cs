using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using MusicHub.Data.Common;

namespace MusicHub.Data.Models;

public class Album
{
    public Album()
    {
        Songs = new HashSet<Song>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ConstraintConstants.AlbumNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    [Column(TypeName = "Date")]
    public DateTime ReleaseDate { get; set; }

    [NotMapped]
    public decimal Price => this.Songs.Sum(s => s.Price);

    public int? ProducerId { get; set; }

    [ForeignKey(nameof(ProducerId))]
    public virtual Producer? Producer { get; set; }


    public virtual ICollection<Song> Songs { get; set; }
}
