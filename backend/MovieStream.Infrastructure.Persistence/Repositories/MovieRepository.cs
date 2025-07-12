using Microsoft.EntityFrameworkCore;
using MovieStream.Core.Application.Common.Parameters.Movies;
using MovieStream.Core.Application.Features.Movies.Queries.GetAllMovies;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using MovieStream.Core.Domain.Entities;
using MovieStream.Infrastructure.Persistence.Contexts;
using MovieStream.Infrastructure.Persistence.Extensions;

namespace MovieStream.Infrastructure.Persistence.Repositories
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MovieRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<Movie>> GetAllWithFilters(MovieParameters parameters)
        {
            IQueryable<Movie> moviesQuery = _dbContext.Movies
                .AsNoTracking()
                .Include(m => m.Genres)
                .Include(m => m.ProductionCompany);

            moviesQuery = moviesQuery.FilterMovies(parameters);
            moviesQuery = moviesQuery.SearchMovies(parameters.SearchTerm);

            var totalCount = await moviesQuery.CountAsync();
            
            var itemsForCurrentPage = await moviesQuery
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return PagedList<Movie>.ToPagedList(itemsForCurrentPage, totalCount, parameters.PageNumber, parameters.PageSize);
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
