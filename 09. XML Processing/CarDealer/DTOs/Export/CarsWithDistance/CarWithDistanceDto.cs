using System.Xml.Serialization;

namespace CarDealer.DTOs.Export.CarsWithDistance
{
    [XmlType("car")]
    public class CarWithDistanceDto
    {
        [XmlElement("make")]
        public string Make { get; set; } = null!;

        [XmlElement("model")]
        public string Model { get; set; } = null!;

        [XmlElement("traveled-distance")]
        public long TraveledDistance { get; set; }
    }
}
