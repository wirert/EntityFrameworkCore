using System.ComponentModel.DataAnnotations;

namespace CarDealer.DTOs.Import
{
    public class PartDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int SupplierId { get; set; }
    }
}
