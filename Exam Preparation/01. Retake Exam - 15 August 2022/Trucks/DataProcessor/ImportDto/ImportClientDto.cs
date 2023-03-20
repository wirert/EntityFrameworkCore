namespace Trucks.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportClientDto
    {
        [JsonProperty]
        [MinLength(3)]
        [MaxLength(40)]
        [Required]
        public string Name { get; set; } = null!;

        [JsonProperty]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Nationality { get; set; } = null!;

        [Required]
        [MaxLength(15)]
        public string Type { get; set; } = null!;

        [JsonProperty("Trucks")]
        public HashSet<int> TruckIds { get; set; }
    }
}
