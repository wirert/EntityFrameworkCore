namespace FastFood.Services
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using FastFood.Data;
    using FastFood.Models;
    using FastFood.Services.Models.Items;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Models.Categories;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ItemsService : Service<CreateItemDto, ListItemDto>
    {
        public ItemsService(FastFoodContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override async Task AddAsync(CreateItemDto entityDto)
        {
            var item = mapper.Map<Item>(entityDto);
            context.Items.Add(item);
            await context.SaveChangesAsync();
        }

        public override async Task<IList<ListItemDto>> GetAllAsync()
        {
            return await context.Items
                    .ProjectTo<ListItemDto>(mapper.ConfigurationProvider)
                    .ToListAsync();
        }
    }
}
