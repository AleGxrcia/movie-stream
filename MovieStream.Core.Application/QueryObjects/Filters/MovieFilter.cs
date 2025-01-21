using MovieStream.Core.Application.Enums;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.QueryObjects.Filters
{
    public static class MovieFilter
    {
        public static IQueryable<Movie> FilterMoviesBy(this IQueryable<Movie> movies, string filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return movies;

            if (!Enum.TryParse(typeof(FilterByOptions), filterBy, true, out var filterOption))
                throw new ArgumentException($"Invalid filter option: {filterBy}", nameof(filterBy));
            try
            {
                return (FilterByOptions)filterOption switch
                {
                    FilterByOptions.Name => movies.Where(x => x.Name.Contains(filterValue)),
                    FilterByOptions.Genres => movies.Where(x => x.Genres != null && x.Genres.Any(y => y.Name == filterValue)),
                    FilterByOptions.ReleaseDate => movies.Where(x => x.ReleaseDate.Year == int.Parse(filterValue) && x.ReleaseDate <= DateTime.UtcNow),
                    FilterByOptions.ProductionCompany => movies.Where(x => x.ProductionCompany != null && x.ProductionCompany.Name == filterValue),
                    _ => throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null)
                };
            }
            catch (FormatException ex)
            {
                throw new ArgumentException($"Invalid filter value: {filterValue} for filter option: {filterBy}", nameof(filterValue), ex);
            }
        }
    }
}
