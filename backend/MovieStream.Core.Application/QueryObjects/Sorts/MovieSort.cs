using MovieStream.Core.Application.Enums;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.QueryObjects.Sorts
{
    public static class MovieSort
    {
        public static IQueryable<Movie> OrderMoviesBy(this IQueryable<Movie> movies, string sortBy, string sortOrder)
        {
            if (!Enum.TryParse(typeof(SortByOptions), sortBy, true, out var sortOption))
                throw new ArgumentException($"Invalid sort option: {sortBy}", nameof(sortBy));

            var isAscending = sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase);

            return (SortByOptions)sortOption switch
            {
                SortByOptions.Name => isAscending
                    ? movies.OrderBy(x => x.Name)
                    : movies.OrderByDescending(x => x.Name),

                SortByOptions.ReleaseDate => isAscending
                    ? movies.OrderBy(x => x.ReleaseDate)
                    : movies.OrderByDescending(x => x.ReleaseDate),

                SortByOptions.ProductionCompany => isAscending
                    ? movies.OrderBy(x => x.ProductionCompany!.Name)
                    : movies.OrderByDescending(x => x.ProductionCompany!.Name),

                _ => throw new ArgumentOutOfRangeException(nameof(sortBy), sortBy, null)
            };
        }
    }
}
