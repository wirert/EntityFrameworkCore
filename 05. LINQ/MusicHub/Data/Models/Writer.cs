using MusicHub.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models;

public class Writer
{
    public Writer()
    {
        Songs = new HashSet<Song>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ConstraintConstants.WriterNameMaxLength)]
    public string Name { get; set; } = null!;

    [MaxLength(ConstraintConstants.WriterPseudonymMaxLength)]
    public string? Pseudonym { get; set; }

    public virtual ICollection<Song> Songs { get; set; }
}
