namespace MovieStream.Core.Application.Features.TvSeries.Queries.GetAllTvSeries
{
    public class GetAllTvSeriesParameters
    {
        public const int MaxPageSize = 100;

        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; } = "asc";
        public string? FilterBy { get; set; }
        public string? FilterValue { get; set; }
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
