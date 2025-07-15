using Azure.Core;
using Microsoft.EntityFrameworkCore;
using MovieStream.Core.Application.Common.Parameters.Base;
using MovieStream.Core.Application.Common.Parameters.TvSeries;
using MovieStream.Core.Application.Features.TvSeries.Queries.GetAllTvSeries;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.QueryObjects;
using MovieStream.Core.Application.QueryObjects.Filters;
using MovieStream.Core.Application.QueryObjects.Sorts;
using MovieStream.Core.Application.Wrappers;
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

        //public override async Task<PagedList<TvSerie>> GetAllWithFilters(RequestParameters parameters)
        //{
        //    var tvSerieParams = parameters as TvSerieParameters;

        //    IQueryable<TvSerie> tvSeriesQuery = _dbContext.TvSeries
        //        .AsNoTracking()
        //        .Include(tv => tv.Genres)
        //        .Include(tv => tv.Seasons)
        //            .ThenInclude(tv => tv.Episodes)
        //        .Include(tv => tv.ProductionCompany);
        //}

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
