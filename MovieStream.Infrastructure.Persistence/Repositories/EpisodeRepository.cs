using Microsoft.EntityFrameworkCore;
using MovieStream.Core.Application.Features.Episodes.Queries.GetAllEpisodes;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.QueryObjects;
using MovieStream.Core.Application.QueryObjects.Filters;
using MovieStream.Core.Application.QueryObjects.Sorts;
using MovieStream.Core.Domain.Entities;
using MovieStream.Infrastructure.Persistence.Contexts;

namespace MovieStream.Infrastructure.Persistence.Repositories
{
    public class EpisodeRepository : GenericRepository<Episode>, IEpisodeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EpisodeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(List<Episode>, int totalCount)> GetAllWithFilters(GetAllEpisodesParameters filters)
        {
            var query = _dbContext.Episodes
                .AsQueryable()
                .AsNoTracking();

            if (!string.IsNullOrEmpty(filters.FilterBy) && !string.IsNullOrEmpty(filters.FilterValue))
            {
                query.FilterEpisodesBy(filters.FilterBy, filters.FilterValue);
            }

            if (!string.IsNullOrEmpty(filters.SortColumn))
            {
                query.OrderEpisodesBy(filters.SortColumn, filters.SortOrder);
            }

            var totalCount = await query.CountAsync();

            var episodes = await query
                .Page(filters.PageNumber, filters.PageSize)
                .ToListAsync();

            return (episodes, totalCount);
        }

        public async Task<Episode?> GetByIdWithInclude(int id)
        {
            return await _dbContext.Episodes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
