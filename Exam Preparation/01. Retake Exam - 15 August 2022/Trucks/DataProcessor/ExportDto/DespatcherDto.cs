namespace Trucks.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Despatcher")]
    public class DespatcherDto
    {
        [XmlElement("DespatcherName")]
        public string Name { get; set; }

        [XmlArray("Trucks")]
        public TruckDto[] Trucks { get; set; }

        [XmlAttribute("TrucksCount")]
        public int TrucksCount {get; set; }
    }
}
