using System.Security.Cryptography;

namespace CarDealer.DTOs.Import
{
    public class SaleDto
    {
        public int CarId { get; set; }

        public int CustomerId { get; set; }

        public decimal Discount { get; set; }
    }
}
