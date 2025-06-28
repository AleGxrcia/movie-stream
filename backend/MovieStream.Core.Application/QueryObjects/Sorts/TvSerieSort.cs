using MovieStream.Core.Application.Enums;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.QueryObjects.Sorts
{
    public static class TvSerieSort
    {
        public static IQueryable<TvSerie> OrderTvSeriesBy(this IQueryable<TvSerie> tvSeries, string sortBy, string sortOrder)
        {
            if (!Enum.TryParse(typeof(SortByOptions), sortBy, true, out var sortOption))
                throw new ArgumentException($"Invalid sort option: {sortBy}", nameof(sortBy));

            var isAscending = sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase);

            return (SortByOptions)sortOption switch
            {
                SortByOptions.Name => isAscending
                    ? tvSeries.OrderBy(x => x.Name)
                    : tvSeries.OrderByDescending(x => x.Name),

                SortByOptions.ReleaseDate => isAscending
                    ? tvSeries.OrderBy(x => x.ReleaseDate)
                    : tvSeries.OrderByDescending(x => x.ReleaseDate),

                SortByOptions.ProductionCompany => isAscending
                    ? tvSeries.OrderBy(x => x.ProductionCompany!.Name)
                    : tvSeries.OrderByDescending(x => x.ProductionCompany!.Name),

                _ => throw new ArgumentOutOfRangeException(nameof(sortBy), sortBy, null)
            };
        }
    }
}
