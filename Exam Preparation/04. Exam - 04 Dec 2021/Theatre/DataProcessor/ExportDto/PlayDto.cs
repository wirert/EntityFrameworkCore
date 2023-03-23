namespace Theatre.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Play")]
    public class PlayDto
    {
        [XmlAttribute("Title")]
        public string Title { get; set; } = null!;

        [XmlAttribute("Duration")]
        public string Duration { get; set; } = null!;

        [XmlAttribute("Rating")]
        public string Rating { get; set; } = null!;

        [XmlAttribute("Genre")]
        public string Genre { get; set; } = null!;

        [XmlArray("Actors")]
        public ActorDto[] Actors { get; set; }
    }
}
//Title="A Raisin in the Sun" Duration="01:40:00" Rating="5.4" Genre="Drama">
 //   < Actors >
