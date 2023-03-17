using System.Xml.Serialization;

namespace CarDealer.DTOs.Export.SalesWithDiscounts
{
    [XmlType("sale")]
    public class SaleDto
    {
        [XmlElement("car")]
        public CarSaleDto Car { get; set; }

        [XmlElement("discount")]
        public decimal Discount { get; set; }

        [XmlElement("customer-name")]
        public string CustomerName { get; set; }

        [XmlElement("price")]
        public string Price { get; set; }

        [XmlElement("price-with-discount")]
        public string PriceWithDiscount { get; set; }
    }
}
