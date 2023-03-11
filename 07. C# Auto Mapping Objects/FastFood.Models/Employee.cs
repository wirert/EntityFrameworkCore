namespace FastFood.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Employee
    {
        public int Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; } = null!;

       
        public int Age { get; set; }

        [MaxLength(30)]
        public string Address { get; set; } = null!;

        public int PositionId { get; set; }

        [Required]
        public Position Position { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = new List<Order>(); 
    }
}