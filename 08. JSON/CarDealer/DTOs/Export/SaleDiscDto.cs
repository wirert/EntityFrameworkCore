using Newtonsoft.Json;

namespace CarDealer.DTOs.Export
{
    [JsonObject]
    public class SaleDiscDto
    {
        [JsonProperty("car")]
        public CarWithPartsDto CarInfo { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("discount")]
        public decimal Discount { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("priceWithDiscount")]
        public string PriceWithDiscount => (this.Price * (100 - this.Discount) / 100).ToString("F2");
    }
}
