using Newtonsoft.Json;

namespace ProductShop.DTOs.Export
{
    public class ExportSoldProductsCountDto
    {
        [JsonProperty("count")]
        public int Count => Products.Length;

        [JsonProperty("products")]
        public ExportSoldProductForUserProductsDto[] Products { get; set; }
    }
}
