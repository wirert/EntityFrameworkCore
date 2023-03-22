namespace Artillery.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Gun")]
    public class GunDto
    {
        [XmlAttribute]
        public string Manufacturer { get; set; } = null!;

        [XmlAttribute]
        public string GunType { get; set; }

        [XmlAttribute]
        public int GunWeight { get; set; }

        [XmlAttribute]
        public double BarrelLength { get; set; }

        [XmlAttribute]
        public int Range { get; set; }

        [XmlArray]
        public CountryDto[] Countries { get; set; }
    }
}
