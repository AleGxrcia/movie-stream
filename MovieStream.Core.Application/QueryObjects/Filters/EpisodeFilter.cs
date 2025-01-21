using MovieStream.Core.Application.Enums;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.QueryObjects.Filters
{
    public static class EpisodeFilter
    {
        public static IQueryable<Episode> FilterEpisodesBy(this IQueryable<Episode> episodes, string filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return episodes;

            if (!Enum.TryParse(typeof(FilterByOptions), filterBy, true, out var filterOption))
                throw new ArgumentException($"Invalid filter option: {filterBy}", nameof(filterBy));

            try
            {
                return (FilterByOptions)filterOption switch
                {
                    FilterByOptions.Name => episodes.Where(x => x.Name.Contains(filterValue)),
                    FilterByOptions.ReleaseDate => episodes.Where(x => x.ReleaseDate.Year == int.Parse(filterValue) && x.ReleaseDate <= DateTime.UtcNow),
                    FilterByOptions.Runtime => episodes.Where(x => x.Runtime.TotalMinutes == double.Parse(filterValue)),
                    FilterByOptions.SeasonNumber => episodes.Where(x => x.Season != null && x.Season.SeasonNumber == int.Parse(filterValue)),
                    FilterByOptions.EpisodeNumber => episodes.Where(x => x.EpisodeNumber == int.Parse(filterValue)),
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
