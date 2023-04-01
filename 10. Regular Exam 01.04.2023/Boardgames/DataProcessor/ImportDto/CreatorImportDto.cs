using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Creator")]
    public class CreatorImportDto
    {
        [XmlElement("FirstName")]
        [Required]
        [StringLength(7, MinimumLength = 2)]
        public string? FirstName { get; set; }

        [XmlElement("LastName")]
        [Required]
        [StringLength(7, MinimumLength = 2)]
        public string? LastName { get; set; }

        [XmlArray("Boardgames")]
        public BoardgameImportDto[] Boardgames { get; set; }
    }
}

