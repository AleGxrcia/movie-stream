using Microsoft.EntityFrameworkCore;
using MovieStream.Core.Application.Common.Parameters.Base;
using MovieStream.Core.Application.Common.Parameters.TvSeries;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using MovieStream.Core.Domain.Entities;
using MovieStream.Infrastructure.Persistence.Contexts;
using MovieStream.Infrastructure.Persistence.Extensions;

namespace MovieStream.Infrastructure.Persistence.Repositories
{
    public class TvSerieRepository : GenericRepository<TvSerie>, ITvSerieRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TvSerieRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<PagedList<TvSerie>> GetAllWithFilters(RequestParameters parameters)
        {
            var tvSerieParams = parameters as TvSerieParameters;

            IQueryable<TvSerie> tvSeriesQuery = _dbContext.TvSeries
                .AsNoTracking()
                .AsSplitQuery()
                .Include(tv => tv.Genres)
                .Include(tv => tv.Seasons)
                    .ThenInclude(tv => tv.Episodes)
                .Include(tv => tv.ProductionCompany);

            tvSeriesQuery.FilterTvSeries(tvSerieParams);
            tvSeriesQuery.SearchTvSeries(tvSerieParams.SearchTerm);

            var totalCount = await tvSeriesQuery.CountAsync();

            var tvSeries = await tvSeriesQuery
                .Skip((tvSerieParams.PageNumber - 1) * tvSerieParams.PageSize)
                .Take(tvSerieParams.PageSize)
                .ToListAsync();

            return PagedList<TvSerie>.ToPagedList(tvSeries, totalCount, tvSerieParams.PageNumber, tvSerieParams.PageSize);
        }

        public async Task<TvSerie?> GetByIdWithInclude(int id)
        {
            var query = _dbContext.TvSeries
                .AsNoTracking()
                .AsSplitQuery()
                .Include(tv => tv.Genres)
                .Include(tv => tv.Seasons)
                    .ThenInclude(tv => tv.Episodes)
                .Include(m => m.ProductionCompany);

            var tvSerie = await query.FirstOrDefaultAsync(m => m.Id == id);

            return tvSerie;
        }
    }
}
