namespace FastFood.Services.Models.Employees
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RegisterEmployeeDto
    {

        public string Name { get; set; } = null!;
                
        public int Age { get; set; }

        public int PositionId { get; set; }

        public string PositionName { get; set; } = null!;

        public string Address { get; set; } = null!;
    }
}
