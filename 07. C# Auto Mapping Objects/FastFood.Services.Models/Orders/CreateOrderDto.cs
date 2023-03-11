﻿namespace FastFood.Services.Models.Orders
{
    public class CreateOrderDto
    {
        public string Customer { get; set; } = null!;

        public int ItemId { get; set; }

        public int EmployeeId { get; set; }

        public int Quantity { get; set; }
    }
}
