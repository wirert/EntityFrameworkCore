namespace Footballers.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    public class ImportTeamDto
    {
        [Required]
        [StringLength(40, MinimumLength = 3)]
        [RegularExpression(@"[a-zA-z.\-\d\s]+")]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Nationality { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue)]
        public int Trophies { get; set; }

        [JsonProperty("Footballers")]
        public HashSet<int> FootballersIds { get; set; }
    }
}
