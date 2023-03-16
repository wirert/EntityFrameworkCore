using ProductShop.Models;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.SoldProducts
{
    [XmlType("User")]
    [Serializable]
    public class UserSoldProductsDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlArray("soldProducts")]
        public SoldProductDto[] ProductsSold { get; set; }
    }
}
