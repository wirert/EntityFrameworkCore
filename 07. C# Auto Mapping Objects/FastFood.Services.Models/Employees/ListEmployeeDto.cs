namespace FastFood.Services.Models.Employees
{
    public class ListEmployeeDto
    {
        public string Name { get; set; } = null!;

        public int Age { get; set; }

        public string Address { get; set; } = null!;

        public string PositionName { get; set; } = null!;
    }
}
