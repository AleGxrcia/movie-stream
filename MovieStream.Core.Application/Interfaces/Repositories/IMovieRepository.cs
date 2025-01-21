using MovieStream.Core.Application.Features.Movies.Queries.GetAllMovies;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.Interfaces.Repositories
{
    public interface IMovieRepository : IGenericRepository<Movie>
    {
        Task<(List<Movie>, int totalCount)> GetAllWithFilters(GetAllMoviesParameters filters);
        Task<Movie?> GetByIdWithInclude(int id);
    }
}
