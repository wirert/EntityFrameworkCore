namespace FastFood.Core.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    using FastFood.Services.Interfaces;
    using FastFood.Services.Models.Positons;
    using ViewModels.Positions;

    public class PositionsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IService<CreatePositionDto, ListPositionDto> service;

        public PositionsController(IMapper mapper, IService<CreatePositionDto, ListPositionDto> service)
        {
            this.mapper = mapper;
            this.service = service;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePositionInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            var positionDto = mapper.Map<CreatePositionDto>(model);

            await service.AddAsync(positionDto);

            return RedirectToAction("All", "Positions");
        }

        public async Task<IActionResult> All()
        {
            var positionsDto = await service.GetAllAsync();

            var positionViews = new List<PositionsAllViewModel>();

            foreach (var position in positionsDto)
            {
                positionViews.Add(mapper.Map<PositionsAllViewModel>(position));
            }

            return View(positionViews);
        }
    }
}
