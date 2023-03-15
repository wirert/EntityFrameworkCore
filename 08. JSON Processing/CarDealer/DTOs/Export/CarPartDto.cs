using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CarDealer.DTOs.Export
{
    [JsonObject]
    public class CarPartDto
    {
        public string Name { get; set; } = null!;

        public string Price { get; set; }
    }
}
