using MusicHub.Data.Common;
using MusicHub.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHub.Data.Models;

public class Song
{
    public Song()
    {
        SongPerformers = new HashSet<SongPerformer>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ConstraintConstants.SongNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    public TimeSpan Duration { get; set; }

    [Required]
    [Column(TypeName = "Date")]
    public DateTime CreatedOn { get; set; }

    [Required]
    public Genre Genre { get; set; }

    public int? AlbumId { get; set; }

    [ForeignKey(nameof(AlbumId))]
    public virtual Album? Album { get; set; }

    public int WriterId  { get; set; }

    [ForeignKey(nameof(WriterId))]
    public virtual Writer Writer { get; set; } = null!;

    [Required]
    public decimal Price { get; set; }

    public ICollection<SongPerformer> SongPerformers  { get; set; }
}