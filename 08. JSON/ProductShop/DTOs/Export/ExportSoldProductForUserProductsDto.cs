using Newtonsoft.Json;

namespace ProductShop.DTOs.Export
{
    [JsonObject]
    public class ExportSoldProductForUserProductsDto
    {
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
