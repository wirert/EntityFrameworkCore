namespace FastFood.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Item
    {
        public int Id { get; set; }

        [MaxLength(30)]
        public string? Name { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public decimal Price { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}