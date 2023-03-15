using Newtonsoft.Json;

namespace ProductShop.DTOs.Import
{
    [JsonObject]
    public class ImportCategoryProductDto
    {
        [JsonProperty(ItemIsReference = true)]
        public int CategoryId { get; set; }

        [JsonProperty(ItemIsReference = true)]
        public int ProductId { get; set; }
    }
}
