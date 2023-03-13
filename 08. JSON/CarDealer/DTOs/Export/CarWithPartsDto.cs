using Newtonsoft.Json;

namespace CarDealer.DTOs.Export
{
    [JsonObject]
    public class CarWithPartsDto
    {
        public string Make { get; set; } = null!;

        public string Model { get; set; } = null!;

        public long TraveledDistance { get; set; }
    }
}
