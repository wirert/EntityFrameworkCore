namespace Trucks.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Trucks.Data.Models.Enums;

    [XmlType("Truck")]
    public class ImportTruckDto
    {
        [StringLength(8, MinimumLength = 8)]
        [RegularExpression(@"[A-Z]{2}\d{4}[A-Z]{2}")]
        [Required]
        [XmlElement]
        public string RegistrationNumber { get; set; }

        [XmlElement]
        [Required]
        [StringLength(17, MinimumLength = 17)]
        public string VinNumber { get; set; } = null!;

        [XmlElement]
        [Range(950, 1420)]
        public int TankCapacity { get; set; }

        [XmlElement]
        [Range(5000, 29000)]
        public int CargoCapacity { get; set; }

        [XmlElement]
        [Required]
        [Range(CategoryType.GetValues().Min(), 3)]
        public int CategoryType { get; set; }

        [XmlElement]
        [Required]
        [Range(0, 4)]
        public int MakeType { get; set; }
    }
}
