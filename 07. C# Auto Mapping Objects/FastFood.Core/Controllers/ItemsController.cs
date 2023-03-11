namespace FastFood.Core.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    using Services.Interfaces;
    using Services.Models.Categories;
    using Services.Models.Items;
    using ViewModels.Items;

    public class ItemsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IService<CreateItemDto, ListItemDto> itemsService;
        private readonly IService<CreateCategoryDto, ListCategoryDto> categoriesService;


        public ItemsController(IMapper mapper, IService<CreateItemDto, ListItemDto> itemsService, IService<CreateCategoryDto, ListCategoryDto> categoriesService)
        {
            this.mapper = mapper;
            this.itemsService = itemsService;
            this.categoriesService = categoriesService;
        }

        public async Task<IActionResult> Create()
        {
            var categories = await categoriesService.GetAllAsync();

            var viewCategories = new List<CreateItemViewModel>();

            foreach (var category in categories)
            {
                viewCategories.Add(mapper.Map<CreateItemViewModel>(category));
            }

            return View(viewCategories);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateItemInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create", "Items");
            }

            var itemToPass = mapper.Map<CreateItemDto>(model);

            await itemsService.AddAsync(itemToPass);

            return RedirectToAction("All", "Items");
        }

        public async Task<IActionResult> All()
        {
            var items = await itemsService.GetAllAsync();

            var itemsToPass = new List<ItemsAllViewModels>();

            foreach (var item in items)
            {
                itemsToPass.Add(mapper.Map<ItemsAllViewModels>(item));
            }

            return View(itemsToPass);
        }
    }
}
