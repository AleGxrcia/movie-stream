using MovieStream.Core.Application.Enums;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.QueryObjects
{
    public static class TvSerieFilter
    {
        public static IQueryable<TvSerie> FilterSeriesBy(this IQueryable<TvSerie> tvSeries, FilterBy filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return tvSeries;

            switch (filterBy)
            {
                case FilterBy.NoFilter:
                    return tvSeries;
                case FilterBy.ByGenres:
                    return tvSeries.Where(x => x.Genres
                        .Any(y => y.Name == filterValue));
                case FilterBy.ByPublicationYear:
                    var filterYear = int.Parse(filterValue);
                    return tvSeries.Where(x => x.ReleaseDate.Year == filterYear
                                             && x.ReleaseDate <= DateTime.UtcNow);
                case FilterBy.ByProductionCompany:
                    return tvSeries.Where(
                        x => x.ProductionCompany.Name == filterValue);
                default:
                    throw new ArgumentOutOfRangeException
                        (nameof(filterBy), filterBy, null);
            }
        }
    }
}
