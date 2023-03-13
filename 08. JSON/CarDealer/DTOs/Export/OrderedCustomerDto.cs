using Newtonsoft.Json;

namespace CarDealer.DTOs.Export
{
    [JsonObject]
    public class OrderedCustomerDto
    {
        public string Name { get; set; } = null!;

        public string BirthDate { get; set; }

        public bool IsYoungDriver { get; set; }
    }
}
