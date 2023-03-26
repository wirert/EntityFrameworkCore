namespace VaporStore.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    public class UserImportDto
    {
        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string? Username { get; set; }

        [Required]        
        [RegularExpression(@"^[A-Z][a-z]+ [A-Z][a-z]+$")]
        public string? FullName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [Range(3, 103)]
        public int Age { get; set; }

        [Required]
        public CardImportDto[]? Cards { get; set; }
    }
}
