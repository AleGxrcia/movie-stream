using MovieStream.Core.Application.Enums;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.QueryObjects
{
    public static class MovieFilter
    {
        public static IQueryable<Movie> FilterSeriesBy(this IQueryable<Movie> movies, FilterBy filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return movies;

            switch (filterBy)
            {
                case FilterBy.NoFilter:
                    return movies;
                case FilterBy.ByGenres:
                    return movies.Where(x => x.Genres
                        .Any(y => y.Name == filterValue));
                case FilterBy.ByPublicationYear:
                    var filterYear = int.Parse(filterValue);
                    return movies.Where(x => x.ReleaseDate.Year == filterYear
                                             && x.ReleaseDate <= DateTime.UtcNow);
                case FilterBy.ByProductionCompany:
                    return movies.Where(
                        x => x.ProductionCompany.Name == filterValue);
                default:
                    throw new ArgumentOutOfRangeException
                        (nameof(filterBy), filterBy, null);
            }
        }
    }
}
