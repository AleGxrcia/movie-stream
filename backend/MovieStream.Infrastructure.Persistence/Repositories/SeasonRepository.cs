using Microsoft.EntityFrameworkCore;
using MovieStream.Core.Application.Features.Seasons.Queries.GetAllSeasons;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.QueryObjects;
using MovieStream.Core.Application.QueryObjects.Filters;
using MovieStream.Core.Application.QueryObjects.Sorts;
using MovieStream.Core.Domain.Entities;
using MovieStream.Infrastructure.Persistence.Contexts;

namespace MovieStream.Infrastructure.Persistence.Repositories
{
    public class SeasonRepository : GenericRepository<Season>, ISeasonRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SeasonRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(List<Season>, int totalCount)> GetAllWithFilters(GetAllSeasonsParameters filters)
        {
            var query = _dbContext.Seasons
                .AsQueryable()
                .AsNoTracking();

            if (!string.IsNullOrEmpty(filters.FilterBy) && !string.IsNullOrEmpty(filters.FilterValue))
            {
                query.FilterSeasonsBy(filters.FilterBy, filters.FilterValue);
            }

            if (!string.IsNullOrEmpty(filters.SortColumn))
            {
                query.OrderSeasonsBy(filters.SortColumn, filters.SortOrder);
            }

            var totalCount = await query.CountAsync();

            var seasons = await query
                .Page(filters.PageNumber, filters.PageSize)
                .ToListAsync();

            return (seasons, totalCount);
        }

        public async Task<Season?> GetByIdWithInclude(int id)
        {
            return await _dbContext.Seasons
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
