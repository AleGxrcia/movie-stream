namespace MovieStream.Core.Application.QueryObjects
{
    public static class GenericPaging
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> query, int pageNumZeroStart, int pageSize)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException
                    (nameof(pageSize), "pageSize must be greater than zero.");
            }

            if (pageNumZeroStart < 0)
            {
                throw new ArgumentOutOfRangeException
                    (nameof(pageNumZeroStart), "pageNumZeroStart cannot be negative.");
            }

            if (pageNumZeroStart != 0)
            {
                query = query.Skip((pageNumZeroStart - 1) * pageSize);
            }

            return query.Take(pageSize);
        }
    }
}
