namespace MovieStream.Core.Application.Common.Parameters.Base
{
    public abstract class RequestParameters
    {
        const int MaxPageSize = 50;
        const int DefaultPageSize = 10;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = DefaultPageSize;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value < 1 ? DefaultPageSize : value;
        }

        public string? OrderBy { get; set; }
        public string? SearchTerm { get; set; }
    }
}
