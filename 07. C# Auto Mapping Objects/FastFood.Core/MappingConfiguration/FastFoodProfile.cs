namespace FastFood.Core.MappingConfiguration
{
    using AutoMapper;

    using FastFood.Core.ViewModels.Categories;
    using FastFood.Core.ViewModels.Employees;
    using FastFood.Core.ViewModels.Items;
    using FastFood.Core.ViewModels.Orders;
    using FastFood.Models;
    using FastFood.Services.Models.Categories;
    using FastFood.Services.Models.Employees;
    using FastFood.Services.Models.Items;
    using FastFood.Services.Models.Orders;
    using FastFood.Services.Models.Positons;
    using ViewModels.Positions;

    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            //Positions
            this.CreateMap<CreatePositionInputModel, CreatePositionDto>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.PositionName));
            this.CreateMap<CreatePositionDto, Position>();
            this.CreateMap<Position, ListPositionDto>();
            this.CreateMap<ListPositionDto, PositionsAllViewModel>();

            //Categories
            this.CreateMap<CreateCategoryDto, Category>();
            this.CreateMap<CreateCategoryInputModel, CreateCategoryDto>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.CategoryName));
            this.CreateMap<Category, ListCategoryDto>();
            this.CreateMap<ListCategoryDto, CategoryAllViewModel>();

            //Items
            this.CreateMap<ListCategoryDto, CreateItemViewModel>()
                .ForMember(x => x.CategoryId, y => y.MapFrom(s => s.Id));
            this.CreateMap<CreateItemInputModel, CreateItemDto>();
            this.CreateMap<CreateItemDto, Item>();
            this.CreateMap<Item, ListItemDto>()
                .ForMember(x => x.Category, y => y.MapFrom(s => s.Category.Name));
            this.CreateMap<ListItemDto, ItemsAllViewModels>();

            //Employees
            this.CreateMap<RegisterEmployeeInputModel, RegisterEmployeeDto>();
            this.CreateMap<RegisterEmployeeDto, Employee>();
            this.CreateMap<ListEmployeeDto, EmployeesAllViewModel>()
                .ForMember(x => x.Position, y => y.MapFrom(s => s.PositionName));
            this.CreateMap<Employee, ListEmployeeDto>();
            this.CreateMap<ListPositionDto, RegisterEmployeeViewModel>()
                .ForMember(x => x.PositionId, y => y.MapFrom(s => s.Id));

            //Orders    
            this.CreateMap<CreateOrderViewDto, CreateOrderViewModel>();
            this.CreateMap<CreateOrderInputModel, CreateOrderDto>();
            this.CreateMap<Order, ListOrderDto>()
                .ForMember(x => x.Employee, y => y.MapFrom(s => s.Employee.Name))
                .ForMember(x => x.OrderId, y => y.MapFrom(s => s.Id));
            this.CreateMap<ListOrderDto, OrderAllViewModel>();
        }
    }
}
