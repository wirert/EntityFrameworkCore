namespace Theatre.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    public class ImportTheatreDto
    {
        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(1, 10)]
        public sbyte NumberOfHalls { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Director { get; set; } = null!;


        public ICollection<ImportTicketDto> Tickets { get; set; }
    }
}
