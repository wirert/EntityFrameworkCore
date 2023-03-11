﻿namespace FastFood.Services.Models.Items
{
    public class CreateItemDto
    {
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int CategoryId { get; set; }
    }
}
