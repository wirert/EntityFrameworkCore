using System.ComponentModel.DataAnnotations;

namespace ProductShop.DTOs.Import
{
    public class ImportUserDto
    {
        public string? FirstName { get; set; }

        [Required]
        public string LastName { get; set; } = null!;

        public int? Age { get; set; }
    }
}
