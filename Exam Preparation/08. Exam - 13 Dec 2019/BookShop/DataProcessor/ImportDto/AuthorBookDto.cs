namespace BookShop.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class AuthorBookDto
    {
        [JsonProperty("Id",NullValueHandling = NullValueHandling.Ignore)]
        [Required]
        [Range(1, int.MaxValue)]
        public int BookId { get; set; }
    }
}
