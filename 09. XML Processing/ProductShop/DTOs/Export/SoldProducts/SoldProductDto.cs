using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.SoldProducts
{
    [XmlType("Product")]
    public class SoldProductDto
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        [XmlElement("price")]
        public decimal Price { get; set; }
    }
}
