﻿namespace ProductShop.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;

    public class Product
    {
        public Product()
        {
            CategoriesProducts = new List<CategoryProduct>();
        }

        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int SellerId { get; set; }
        public User Seller { get; set; } = null!;

        [AllowNull]
        public int? BuyerId { get; set; }
        public User? Buyer { get; set; }

        public ICollection<CategoryProduct> CategoriesProducts { get; set; }
    }
}