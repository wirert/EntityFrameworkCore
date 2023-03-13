using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CarDealer.DTOs.Export
{
    [JsonObject]
    public class CustomerSalesDto
    {
        [JsonProperty("fullName")]
        public string Name { get; set; } = null!;

        [JsonProperty("boughtCars")]
        public int BoughtCars { get; set; }

        [JsonProperty("spentMoney")]
        public decimal[] SpentMoney { get; set; }

    }
}
