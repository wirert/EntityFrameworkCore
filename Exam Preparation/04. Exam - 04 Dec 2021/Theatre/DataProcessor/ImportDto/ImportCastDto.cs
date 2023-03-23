namespace Theatre.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Cast")]
    public class ImportCastDto
    {
        [XmlElement]
        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string FullName { get; set; } = null!;

        [XmlElement("IsMainCharacter",IsNullable =false)]
        [Required]
        public bool IsMainCharacter { get; set; }

        [XmlElement("PhoneNumber", IsNullable =false)]
        [Required]
        [StringLength(15)]
        [RegularExpression(@"\+44-[\d]{2}-[\d]{3}-[\d]{4}")]
        public string PhoneNumber { get; set; } = null!;

        [XmlElement]
        [Required]
        public int PlayId { get; set; }
    }
}
