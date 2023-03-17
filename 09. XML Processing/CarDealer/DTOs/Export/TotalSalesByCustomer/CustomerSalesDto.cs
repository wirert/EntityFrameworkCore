using System.Xml.Serialization;

namespace CarDealer.DTOs.Export.TotalSalesByCustomer
{
    [XmlType("customer")]
    public class CustomerSalesDto
    {
        [XmlAttribute("full-name")]
        public string Name { get; set; } = null!;

        [XmlAttribute("bought-cars")]
        public int BoughtCars { get; set; }

        [XmlAttribute("spent-money")]
        public string SpentMoney { get; set; }            
        
    }
}
