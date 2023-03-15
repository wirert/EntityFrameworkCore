using Newtonsoft.Json;
using ProductShop.Models;

namespace ProductShop.DTOs.Export
{
    [JsonObject]
    public class ExportUserWithProductsDto
    {
        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; } = null!;

        [JsonProperty("age")]
        public int? Age { get; set; }

        [JsonProperty("soldProducts")]
        public ExportSoldProductsCountDto SoldProducts { get; set; }


    }
}
