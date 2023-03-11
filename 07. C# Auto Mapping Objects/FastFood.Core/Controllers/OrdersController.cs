namespace FastFood.Core.Controllers
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Data;
    using FastFood.Services.Interfaces;
    using FastFood.Services.Models.Items;
    using FastFood.Services.Models.Orders;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Orders;

    public class OrdersController : Controller
    {
        private readonly IMapper mapper;
        private readonly IService<CreateOrderDto, ListOrderDto> ordersService;

        public OrdersController(IMapper mapper, IService<CreateOrderDto, ListOrderDto> ordersService)
        {
            this.mapper = mapper;
            this.ordersService = ordersService;
        }

        public async Task<IActionResult> Create()
        {
            var viewOrderDto = await ordersService.GetItemsAndEmployeesId();

            var viewOrder = mapper.Map<CreateOrderViewModel>(viewOrderDto);

            return View(viewOrder);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            var orderDto = mapper.Map<CreateOrderDto>(model);

            await ordersService.AddAsync(orderDto);

            return RedirectToAction("All", "Orders");
        }

        public async Task<IActionResult> All()
        {
            var allOrderDto = await ordersService.GetAllAsync();

            var allOrdersView = new List<OrderAllViewModel>();

            foreach (var order in allOrderDto)
            {
                allOrdersView.Add(mapper.Map<OrderAllViewModel>(order));
            }

            return View(allOrdersView);
        }
    }
}
