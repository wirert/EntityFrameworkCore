namespace FastFood.Services.Models.Items
{
    public class ListItemDto
    {
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public string Category { get; set; } = null!;
    }
}
