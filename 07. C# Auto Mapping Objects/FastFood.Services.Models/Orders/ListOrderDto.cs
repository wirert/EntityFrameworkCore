namespace FastFood.Services.Models.Orders
{
    public class ListOrderDto
    {
        public int OrderId { get; set; }

        public string Customer { get; set; } = null!;

        public string Employee { get; set; } = null!;

        public string DateTime { get; set; } = null!;
    }
}
