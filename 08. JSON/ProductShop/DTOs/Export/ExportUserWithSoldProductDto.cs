using Newtonsoft.Json;
using ProductShop.Models;

namespace ProductShop.DTOs.Export
{
    [JsonObject]
    public class ExportUserWithSoldProductDto
    {
        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; } = null!;

        [JsonProperty("soldProducts")]
        public ExportSoldProductDto[] SoldProducts { get; set; }
    }
}
