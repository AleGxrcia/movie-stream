using MovieStream.Core.Application.Enums;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.QueryObjects.Filters
{
    public static class TvSerieFilter
    {
        public static IQueryable<TvSerie> FilterTvSeriesBy(this IQueryable<TvSerie> tvSeries, string filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return tvSeries;

            if (!Enum.TryParse(typeof(FilterByOptions), filterBy, true, out var filterOption))
                throw new ArgumentException($"Invalid filter option: {filterBy}", nameof(filterBy));

            try
            {
                return (FilterByOptions)filterOption switch
                {
                    FilterByOptions.Name => tvSeries.Where(x => x.Name.Contains(filterValue)),
                    FilterByOptions.Genres => tvSeries.Where(x => x.Genres != null && x.Genres.Any(y => y.Name == filterValue)),
                    FilterByOptions.ReleaseDate => tvSeries.Where(x => x.ReleaseDate.Year == int.Parse(filterValue) && x.ReleaseDate <= DateTime.UtcNow),
                    FilterByOptions.ProductionCompany => tvSeries.Where(x => x.ProductionCompany != null && x.ProductionCompany.Name == filterValue),
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
