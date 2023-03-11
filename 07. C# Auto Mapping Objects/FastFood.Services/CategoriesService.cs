namespace FastFood.Services
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    using FastFood.Data;
    using FastFood.Models;
    using FastFood.Services.Models.Categories;

    public class CategoriesService : Service<CreateCategoryDto, ListCategoryDto>
    {
        public CategoriesService(FastFoodContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override async Task AddAsync(CreateCategoryDto categoryDto)
        {
            var category = mapper.Map<Category>(categoryDto);
            context.Categories.Add(category);
            await context.SaveChangesAsync();
        }

        public override async Task<IList<ListCategoryDto>> GetAllAsync()
        {
            var categories = await this.context.Categories
                .ProjectTo<ListCategoryDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            return categories;
        }
    }
}