using MusicHub.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models;

public class Producer
{
    public Producer()
    {
        Albums = new HashSet<Album>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ConstraintConstants.ProducerNameMaxLength)]
    public string Name { get; set; } = null!;

    [MaxLength(ConstraintConstants.ProducerPseudonymMaxLength)]
    public string? Pseudonym { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

    public virtual ICollection<Album> Albums { get; set; }
}
