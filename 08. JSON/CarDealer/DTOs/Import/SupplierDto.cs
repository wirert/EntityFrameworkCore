using Newtonsoft.Json;

namespace CarDealer.DTOs.Import
{
    [JsonObject]
    public class SupplierDto
    {
        [JsonRequired]
        public string Name { get; set; } = null!;

        public bool IsImporter { get; set; }
    }
}
