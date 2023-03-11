namespace FastFood.Core.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    using FastFood.Core.ViewModels.Positions;
    using FastFood.Services.Interfaces;
    using FastFood.Services.Models.Employees;
    using FastFood.Services.Models.Positons;
    using ViewModels.Employees;

    public class EmployeesController : Controller
    {
        private readonly IMapper mapper;
        private readonly IService<CreatePositionDto, ListPositionDto> positionsService;
        private readonly IService<RegisterEmployeeDto, ListEmployeeDto> employeesService;

        public EmployeesController(IMapper mapper, IService<CreatePositionDto, ListPositionDto> positionsService, IService<RegisterEmployeeDto, ListEmployeeDto> employeesService)
        {
            this.mapper = mapper;
            this.positionsService = positionsService;
            this.employeesService = employeesService;
        }

        public async Task<IActionResult> Register()
        {
            var positionsDto = await positionsService.GetAllAsync();

            var positionsView = new List<RegisterEmployeeViewModel>();

            foreach (var position in positionsDto)
            {
                positionsView.Add(mapper.Map<RegisterEmployeeViewModel>(position));
            }

            return View(positionsView);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterEmployeeInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            var employee = mapper.Map<RegisterEmployeeDto>(model);

            await employeesService.AddAsync(employee);

            return RedirectToAction("All", "Employees");
        }

        public async Task<IActionResult> All()
        {
            var employees = await employeesService.GetAllAsync();

            var employeesView = new List<EmployeesAllViewModel>();

            foreach (var employee in employees)
            {
                employeesView.Add(mapper.Map<EmployeesAllViewModel>(employee));
            }

            return View(employeesView);
        }
    }
}
