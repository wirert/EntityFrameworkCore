namespace VaporStore.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class GameImportDto
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Required]
        [Range(0.0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public string? ReleaseDate { get; set; }

        [Required]
        public string? Developer { get; set; }

        [Required]
        public string? Genre { get; set; }

        [Required]
        public string[]? Tags { get; set; }
    }
}
