using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace ProductShop.DTOs.Import
{
    [JsonObject]
    public class ImportCategoryDto
    {
        [NotNull]
        public string Name { get; set; } = null!;
    }
}
