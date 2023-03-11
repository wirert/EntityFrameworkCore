namespace FastFood.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Category
    {
        public int Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; } = null!;

        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
