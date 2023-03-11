namespace FastFood.Services
{
    using AutoMapper;
    using FastFood.Data;
    using FastFood.Services.Interfaces;
    using FastFood.Services.Models.Orders;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public abstract class Service<T,S> : IService<T, S>
    {
        private protected readonly FastFoodContext context;
        private protected readonly IMapper mapper;

        public Service(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public abstract Task AddAsync(T entityDto);

        public abstract Task<IList<S>> GetAllAsync();

        public virtual async Task<CreateOrderViewDto> GetItemsAndEmployeesId()
        {
            return new CreateOrderViewDto
            {
                Items = await context.Items.Select(i => i.Id).ToListAsync(),
                Employees = await context.Employees.Select(e => e.Id).ToListAsync(),
            };
        }
    }
}
