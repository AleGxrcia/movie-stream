using MovieStream.Core.Application.Enums;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.QueryObjects.Sorts
{
    public static class SeasonSort
    {
        public static IQueryable<Season> OrderSeasonsBy(this IQueryable<Season> seasons, string sortBy, string sortOrder)
        {
            if (!Enum.TryParse(typeof(SortByOptions), sortBy, true, out var sortOption))
                throw new ArgumentException($"Invalid sort option: {sortBy}", nameof(sortBy));

            var isAscending = sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase);

            return (SortByOptions)sortOption switch
            {
                SortByOptions.TvSerieName => isAscending
                    ? seasons.OrderBy(x => x.TvSerie != null ? x.TvSerie.Name : string.Empty)
                    : seasons.OrderByDescending(x => x.TvSerie != null ? x.TvSerie.Name : string.Empty),

                SortByOptions.SeasonNumber => isAscending
                    ? seasons.OrderBy(x => x.SeasonNumber)
                    : seasons.OrderByDescending(x => x.SeasonNumber),

                SortByOptions.ReleaseDate => isAscending
                    ? seasons.OrderBy(x => x.ReleaseDate)
                    : seasons.OrderByDescending(x => x.ReleaseDate),

                _ => throw new ArgumentOutOfRangeException(nameof(sortBy), sortBy, null)
            };
        }
    }
}
