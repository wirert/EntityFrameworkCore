namespace VaporStore.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Purchase")]
    public class PurchiseDto
    {
        [XmlElement("Card")]
        public string CardNumber { get; set; }

        [XmlElement]
        public string Cvc { get; set; }

        [XmlElement]
        public string Date { get; set; }

        [XmlElement("Game")]
        public GameDto Game { get; set; }


    }
}
