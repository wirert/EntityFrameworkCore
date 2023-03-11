namespace FastFood.Core.ViewModels.Orders
{
    using System.ComponentModel.DataAnnotations;

    public class CreateOrderInputModel
    {
        [Required]
        public string Customer { get; set; } = null!;

        public int ItemId { get; set; }

        public int EmployeeId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
