using Boardgames.Data.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Boardgames.DataProcessor.ImportDto
{
    public class SellerImportDto
    {
        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string? Name { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string? Address { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Country { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^www.[A-Za-z\d-]+.com$")]
        public string? Website { get; set; }

        public int[] Boardgames { get; set; }
    }
}


