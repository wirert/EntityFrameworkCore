namespace VaporStore.DataProcessor.ImportDto
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using VaporStore.Data.Models.Enums;

    [XmlType("Purchase")]
    public class PurchaseImportDto
    {
        [XmlAttribute("title")]
        [Required]
        public string? GameName { get; set; }

        [XmlElement("Type")]
        [Required]
        [RegularExpression("Retail|Digital")]
        public string? Type { get; set; }

        [XmlElement("Key")]
        [Required]
        [RegularExpression(@"^[A-Z\d]{4}-[A-Z\d]{4}-[A-Z\d]{4}$")]
        public string? ProductKey { get; set; }

        [XmlElement ("Card")]
        [Required]        
        [RegularExpression(@"^[\d]{4} [\d]{4} [\d]{4} [\d]{4}$")]
        public string? CardNumber { get; set; }

        [XmlElement("Date")]
        [Required]
        public string? Date { get; set; }
    }
}
