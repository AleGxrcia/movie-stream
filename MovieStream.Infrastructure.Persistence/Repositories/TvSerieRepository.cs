using Microsoft.EntityFrameworkCore;
using MovieStream.Core.Application.Features.TvSeries.Queries.GetAllTvSeries;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.QueryObjects;
using MovieStream.Core.Application.QueryObjects.Filters;
using MovieStream.Core.Application.QueryObjects.Sorts;
using MovieStream.Core.Domain.Entities;
using MovieStream.Infrastructure.Persistence.Contexts;

namespace MovieStream.Infrastructure.Persistence.Repositories
{
    public class TvSerieRepository : GenericRepository<TvSerie>, ITvSerieRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TvSerieRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(List<TvSerie>, int totalCount)> GetAllWithFilters(GetAllTvSeriesParameters filters)
        {
            var query = _dbContext.TvSeries
                .AsQueryable()
                .AsNoTracking()
                .Include(tv => tv.Genres)
                .Include(tv => tv.Seasons)
                    .ThenInclude(tv => tv.Episodes)
                .Include(m => m.ProductionCompany);

            if (!string.IsNullOrEmpty(filters.FilterBy) && !string.IsNullOrEmpty(filters.FilterValue))
            {
                query.FilterTvSeriesBy(filters.FilterBy, filters.FilterValue);
            }

            if (!string.IsNullOrEmpty(filters.SortColumn))
            {
                query.OrderTvSeriesBy(filters.SortColumn, filters.SortOrder);
            }

            var totalCount = await query.CountAsync();

            var tvSeries = await query
                .Page(filters.PageNumber, filters.PageSize)
                .ToListAsync();

            return (tvSeries, totalCount);
        }

        public async Task<TvSerie?> GetByIdWithInclude(int id)
        {
            var query = _dbContext.TvSeries
                .AsNoTracking()
                .Include(tv => tv.Genres)
                .Include(tv => tv.Seasons)
                    .ThenInclude(tv => tv.Episodes)
                .Include(m => m.ProductionCompany);

            var tvSerie = await query.FirstOrDefaultAsync(m => m.Id == id);

            return tvSerie;
        }
    }
}
