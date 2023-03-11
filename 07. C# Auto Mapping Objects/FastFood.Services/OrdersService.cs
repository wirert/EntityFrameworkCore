namespace FastFood.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    using FastFood.Data;
    using FastFood.Models;
    using FastFood.Services.Models.Orders;

    public class OrdersService : Service<CreateOrderDto, ListOrderDto>
    {
        public OrdersService(FastFoodContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override async Task AddAsync(CreateOrderDto entityDto)
        {
            var order = new Order
            {
                Customer = entityDto.Customer,
                DateTime = DateTime.UtcNow,
                Type = FastFood.Models.Enums.OrderType.ForHere,
                EmployeeId = entityDto.EmployeeId
            };

            order.OrderItems.Add(new OrderItem
            {
                OrderId = order.Id,
                Quantity = entityDto.Quantity,
                ItemId = entityDto.ItemId
            });

            context.Orders.Add(order);

            await context.SaveChangesAsync();
        }

        public override async Task<IList<ListOrderDto>> GetAllAsync()
        {
            return await context.Orders
                .ProjectTo<ListOrderDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
