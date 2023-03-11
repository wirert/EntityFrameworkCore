namespace FastFood.Core.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    using ViewModels.Categories;
    using Services.Interfaces;
    using Services.Models.Categories;

    public class CategoriesController : Controller
    {
        private readonly IService<CreateCategoryDto, ListCategoryDto> service;
        private readonly IMapper mapper;

        public CategoriesController(IService<CreateCategoryDto, ListCategoryDto> service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create", "Categories");
            }

            var categoryDto = mapper.Map<CreateCategoryDto>(model);

            await this.service.AddAsync(categoryDto);

            return RedirectToAction("All", "Categories");
        }

        public async Task<IActionResult> All()
        {
            var categoryDtos = await this.service.GetAllAsync();

            var viewCategories = new List<CategoryAllViewModel>();

            foreach (var category in categoryDtos)
            {
                viewCategories.Add(mapper.Map<CategoryAllViewModel>(category));
            }

            return View(viewCategories);
        }
    }
}
