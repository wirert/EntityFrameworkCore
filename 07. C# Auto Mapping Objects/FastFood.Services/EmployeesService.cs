namespace FastFood.Services
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using FastFood.Data;
    using FastFood.Models;
    using FastFood.Services.Models.Employees;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class EmployeesService : Service<RegisterEmployeeDto, ListEmployeeDto>
    {
        public EmployeesService(FastFoodContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override async Task AddAsync(RegisterEmployeeDto entityDto)
        {
            var employee = mapper.Map<Employee>(entityDto);
            context.Employees.Add(employee);
            await context.SaveChangesAsync();
        }

        public override async Task<IList<ListEmployeeDto>> GetAllAsync()
        {
            return await context.Employees
                .ProjectTo<ListEmployeeDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
