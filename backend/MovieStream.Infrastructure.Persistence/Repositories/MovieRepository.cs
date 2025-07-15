using Microsoft.EntityFrameworkCore;
using MovieStream.Core.Application.Common.Parameters.Base;
using MovieStream.Core.Application.Common.Parameters.Movies;
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

        public override async Task<PagedList<Movie>> GetAllWithFilters(RequestParameters parameters)
        {
            var movieParams = parameters as MovieParameters;

            IQueryable<Movie> moviesQuery = _dbContext.Movies
                .AsNoTracking()
                .Include(m => m.Genres)
                .Include(m => m.ProductionCompany);

            moviesQuery = moviesQuery.FilterMovies(movieParams);
            moviesQuery = moviesQuery.SearchMovies(movieParams.SearchTerm);

            var totalCount = await moviesQuery.CountAsync();
            
            var itemsForCurrentPage = await moviesQuery
                .Skip((movieParams.PageNumber - 1) * movieParams.PageSize)
                .Take(movieParams.PageSize)
                .ToListAsync();

            return PagedList<Movie>.ToPagedList(itemsForCurrentPage, totalCount, movieParams.PageNumber, movieParams.PageSize);
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
