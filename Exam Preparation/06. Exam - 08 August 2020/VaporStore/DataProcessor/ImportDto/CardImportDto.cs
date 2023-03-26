namespace VaporStore.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    public class CardImportDto
    {
        [Required]
        [StringLength(19)]
        [RegularExpression(@"^[\d]{4} [\d]{4} [\d]{4} [\d]{4}$")]
        public string? Number { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        [RegularExpression(@"^[\d]{3}$")]
        public string? Cvc { get; set; }

        [Required]
        [RegularExpression("Debit|Credit")]
        public string? Type { get; set; }
    }
}
