using MovieStream.Core.Application.Enums;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.QueryObjects
{
    public static class SerieSort
    {
        public static IQueryable<TvSerie> OrderSeriesBy(this IQueryable<TvSerie> tvSeries, OrderByOptions orderByOptions)
        {
            switch (orderByOptions)
            {
                case OrderByOptions.SimpleOrder:
                    return tvSeries.OrderByDescending(
                        x => x.Id);
                case OrderByOptions.ByPublicationDate:
                    return tvSeries.OrderByDescending(
                        x => x.ReleaseDate);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(orderByOptions), orderByOptions, null);
            }
        }
    }
}
