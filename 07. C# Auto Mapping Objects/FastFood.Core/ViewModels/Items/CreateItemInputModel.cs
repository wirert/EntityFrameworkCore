﻿namespace FastFood.Core.ViewModels.Items
{
    using System.ComponentModel.DataAnnotations;

    public class CreateItemInputModel
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; } = null!;

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
