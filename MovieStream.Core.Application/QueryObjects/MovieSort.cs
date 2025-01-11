using MovieStream.Core.Application.Enums;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.QueryObjects
{
    public static class MovieSort
    {
        public static IQueryable<Movie> OrderSeriesBy(this IQueryable<Movie> movies, OrderByOptions orderByOptions)
        {
            switch (orderByOptions)
            {
                case OrderByOptions.SimpleOrder:
                    return movies.OrderByDescending(
                        x => x.Id);
                case OrderByOptions.ByPublicationDate:
                    return movies.OrderByDescending(
                        x => x.ReleaseDate);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(orderByOptions), orderByOptions, null);
            }
        }
    }
}
