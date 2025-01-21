using MovieStream.Core.Application.Enums;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.QueryObjects.Sorts
{
    public static class EpisodeSort
    {
        public static IQueryable<Episode> OrderEpisodesBy(this IQueryable<Episode> Episodes, string sortBy, string sortOrder)
        {
            if (!Enum.TryParse(typeof(SortByOptions), sortBy, true, out var sortOption))
                throw new ArgumentException($"Invalid sort option: {sortBy}", nameof(sortBy));

            var isAscending = sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase);

            return (SortByOptions)sortOption switch
            {
                SortByOptions.Name => isAscending
                    ? Episodes.OrderBy(x => x.Name)
                    : Episodes.OrderByDescending(x => x.Name),

                SortByOptions.EpisodeNumber => isAscending
                    ? Episodes.OrderBy(x => x.EpisodeNumber)
                    : Episodes.OrderByDescending(x => x.EpisodeNumber),

                SortByOptions.ReleaseDate => isAscending
                    ? Episodes.OrderBy(x => x.ReleaseDate)
                    : Episodes.OrderByDescending(x => x.ReleaseDate),

                SortByOptions.Runtime => isAscending
                    ? Episodes.OrderBy(x => x.Runtime)
                    : Episodes.OrderByDescending(x => x.Runtime),

                SortByOptions.SeasonNumber => isAscending
                    ? Episodes.OrderBy(x => x.Season.SeasonNumber).ThenBy(x => x.EpisodeNumber)
                    : Episodes.OrderByDescending(x => x.Season.SeasonNumber).ThenByDescending(x => x.EpisodeNumber),

                _ => throw new ArgumentOutOfRangeException(nameof(sortBy), sortBy, null)
            };
        }
    }
}
