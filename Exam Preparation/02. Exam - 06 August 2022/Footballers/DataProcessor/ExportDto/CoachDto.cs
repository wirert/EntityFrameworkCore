namespace Footballers.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Coach")]
    public class CoachDto
    {
        [XmlElement("CoachName")]
        public string Name { get; set; } = null!;

        [XmlAttribute("FootballersCount")]
        public int FootballersCount { get; set; }

        [XmlArray("Footballers")]
        public FootBallerDto[] Footballers { get; set; }
    }
}
