using MovieStream.Core.Application.Enums;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.QueryObjects.Filters
{
    public static class SeasonFilter
    {
        public static IQueryable<Season> FilterSeasonsBy(this IQueryable<Season> seasons, string filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return seasons;

            if (!Enum.TryParse(typeof(FilterByOptions), filterBy, true, out var filterOption))
                throw new ArgumentException($"Invalid filter option: {filterBy}", nameof(filterBy));
            try
            {
                return (FilterByOptions)filterOption switch
                {
                    FilterByOptions.TvSerieName => seasons.Where(x => x.TvSerie != null && x.TvSerie.Name.Contains(filterValue)),
                    FilterByOptions.ReleaseDate => seasons.Where(x => x.ReleaseDate.Year == int.Parse(filterValue) && x.ReleaseDate <= DateTime.UtcNow),
                    FilterByOptions.SeasonNumber => seasons.Where(x => x.SeasonNumber == int.Parse(filterValue)),
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
