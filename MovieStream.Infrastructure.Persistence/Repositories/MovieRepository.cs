using Microsoft.EntityFrameworkCore;
using MovieStream.Core.Application.Features.Movies.Queries.GetAllMovies;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.QueryObjects;
using MovieStream.Core.Application.QueryObjects.Filters;
using MovieStream.Core.Application.QueryObjects.Sorts;
using MovieStream.Core.Domain.Entities;
using MovieStream.Infrastructure.Persistence.Contexts;

namespace MovieStream.Infrastructure.Persistence.Repositories
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MovieRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(List<Movie>, int totalCount)> GetAllWithFilters(GetAllMoviesParameters filters)
        {
            var query = _dbContext.Movies
                .AsQueryable()
                .AsNoTracking()
                .Include(x => x.Genres)
                .Include(x => x.ProductionCompany);

            if (!string.IsNullOrEmpty(filters.FilterBy) && !string.IsNullOrEmpty(filters.FilterValue))
            {
                query.FilterMoviesBy(filters.FilterBy, filters.FilterValue);
            }

            if (!string.IsNullOrEmpty(filters.SortColumn))
            {
                query.OrderMoviesBy(filters.SortColumn, filters.SortOrder);
            }

            var totalCount = await query.CountAsync();

            var movies = await query
                .Page(filters.PageNumber, filters.PageSize)
                .ToListAsync();

            return (movies, totalCount);
        }

        public async Task<Movie?> GetByIdWithInclude(int id)
        {
            return await _dbContext.Movies
                .AsNoTracking()
                .Include(m => m.Genres)
                .Include(m => m.ProductionCompany)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
