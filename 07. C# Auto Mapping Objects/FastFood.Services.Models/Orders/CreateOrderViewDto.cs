namespace FastFood.Services.Models.Orders
{
    using System.Collections.Generic;

    public class CreateOrderViewDto
    {
        public List<int> Items { get; set; } = null!;

        public List<int> Employees { get; set; } = null!;
    }
}
