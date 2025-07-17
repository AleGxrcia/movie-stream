using MovieStream.Core.Application.Common.Parameters.Movies;
using MovieStream.Core.Application.Common.Parameters.TvSeries;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Infrastructure.Persistence.Extensions
{
    public static class RepositoryExtensions
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

        public static IQueryable<TvSerie> FilterTvSeries(this IQueryable<TvSerie> tvSeries, TvSerieParameters parameters)
        {
            if (parameters.MinReleaseYear.HasValue)
            {
                tvSeries = tvSeries.Where(m => m.ReleaseDate.Year >= parameters.MinReleaseYear.Value);
            }

            if (parameters.MaxReleaseYear.HasValue)
            {
                tvSeries = tvSeries.Where(m => m.ReleaseDate.Year <= parameters.MaxReleaseYear.Value);
            }

            if (parameters.GenreId.HasValue)
            {
                tvSeries = tvSeries.Where(m => m.Genres != null && m.Genres.Any(g => g.Id == parameters.GenreId.Value));
            }

            return tvSeries;
        }

        public static IQueryable<TvSerie> SearchTvSeries(this IQueryable<TvSerie> tvSeries, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return tvSeries;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return tvSeries.Where(tv => tv.Name.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
