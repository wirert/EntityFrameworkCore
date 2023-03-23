namespace Theatre.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Theatre.Data.Models.Enums;

    [XmlType("Play")]
    public class ImportPlayDto
    {
        [XmlElement]
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Title { get; set; } = null!;

        [XmlElement(IsNullable = false)]
        [Required]
        public string Duration { get; set; } = null!;

        [XmlElement("Raiting", IsNullable = false)]
        [Required]
        [Range(0.00, 10.00)]
        public float Rating { get; set; }

        [XmlElement(IsNullable = false)]
        [Required]
        [RegularExpression("Drama|Comedy|Romance|Musical")]
        public string Genre { get; set; } = null!;

        [XmlElement(IsNullable = false)]
        [Required]
        [MaxLength(700)]
        public string Description { get; set; } = null!;

        [XmlElement(IsNullable = false)]
        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Screenwriter { get; set; } = null!;
    }
}
