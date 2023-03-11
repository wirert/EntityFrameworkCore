namespace FastFood.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    using FastFood.Data;
    using FastFood.Models;
    using FastFood.Services.Models.Positons;
    using FastFood.Services.Models.Orders;

    public class PositionsService : Service<CreatePositionDto, ListPositionDto>
    {
        public PositionsService(FastFoodContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override async Task AddAsync(CreatePositionDto entityDto)
        {
            var position = mapper.Map<Position>(entityDto);

            context.Positions.Add(position);
            await context.SaveChangesAsync();
        }

        public override async Task<IList<ListPositionDto>> GetAllAsync()
        {
            return await context.Positions
                .ProjectTo<ListPositionDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }        
    }
}
