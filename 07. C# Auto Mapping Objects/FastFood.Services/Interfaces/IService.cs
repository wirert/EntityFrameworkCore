namespace FastFood.Services.Interfaces
{
    using FastFood.Services.Models.Orders;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IService<T , S> 
    {
        Task AddAsync(T entityDto);

        Task<IList<S>> GetAllAsync();

        Task<CreateOrderViewDto> GetItemsAndEmployeesId();
    }
}
