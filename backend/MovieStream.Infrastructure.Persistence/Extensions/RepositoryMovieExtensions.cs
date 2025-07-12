using MovieStream.Core.Application.Common.Parameters.Movies;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Infrastructure.Persistence.Extensions
{
    public static class RepositoryMovieExtensions
    {
        public static IQueryable<Movie> FilterMovies(this IQueryable<Movie> movies, MovieParameters parameters)
        {
            if (parameters.MinReleaseYear.HasValue)
            {
                movies = movies.Where(m => m.ReleaseDate.Year >= parameters.MinReleaseYear.Value);
            }

            if (parameters.MaxReleaseYear.HasValue)
            {
                movies = movies.Where(m => m.ReleaseDate.Year <= parameters.MaxReleaseYear.Value);
            }

            if (parameters.GenreId.HasValue)
            {
                movies = movies.Where(m => m.Genres != null && m.Genres.Any(g => g.Id == parameters.GenreId.Value));
            }

            return movies;
        }

        public static IQueryable<Movie> SearchMovies(this IQueryable<Movie> movies, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return movies;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            movies = movies.Where(m => (m.Name != null && m.Name.ToLower().Contains(lowerCaseSearchTerm)) ||
                                       (m.Description != null && m.Description.ToLower().Contains(lowerCaseSearchTerm)));

            return movies;
        }
    }
}
