using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Creator")]
    public class CreatorExportDto
    {
        [XmlAttribute("BoardgamesCount")]
        public int BoardgamesCount { get; set; }

        [XmlElement("CreatorName")]
        public string FullName { get; set; }

        [XmlArray("Boardgames")]
        public BoardgameExportDto[] Boardgames { get; set; }
    }
}