namespace Footballers.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Coach")]
    public class ImportCoachDto
    {
        [XmlElement("Name")]
        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [XmlElement("Nationality")]
        [Required]
        public string Nationality { get; set; } = null!;

        public ImportFootballerDto[] Footballers { get; set; }
    }
}
