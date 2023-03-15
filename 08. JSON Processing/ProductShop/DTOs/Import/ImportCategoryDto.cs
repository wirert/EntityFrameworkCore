using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ProductShop.DTOs.Import
{
    [JsonObject]
    public class ImportCategoryDto
    {
        [NotNull]
        [Required]
        public string Name { get; set; } = null!;
    }
}
