using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.CategoriesByProducts
{
    [XmlType("Category")]
    public class CategoryByProductsDto
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("count")]
        public int Count { get; set; }

        [XmlElement("averagePrice")]
        public decimal AveragePrice { get; set; }

        [XmlElement("totalRevenue")]
        public decimal TotalRevenue { get; set; }
    }
}
