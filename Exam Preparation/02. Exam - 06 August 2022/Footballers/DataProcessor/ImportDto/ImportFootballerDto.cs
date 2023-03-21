namespace Footballers.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Footballer")]
    public class ImportFootballerDto
    {
        [XmlElement("Name")]
        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [XmlElement("ContractStartDate")]
        [Required]
        [DisplayFormat(DataFormatString = "dd/MM/yyyy")]

        public string ContractStartDate { get; set; } = null!;

        [XmlElement("ContractEndDate")]
        [Required]
        [DisplayFormat(DataFormatString = "dd/MM/yyyy")]
        public string ContractEndDate { get; set; } = null!;

        [XmlElement("PositionType")]
        [Required]
        [Range(0, 3)]
        public int PositionType { get; set; }

        [XmlElement("BestSkillType")]
        [Required]
        [Range (0, 4)]
        public int BestSkillType { get; set; }
    }
}
