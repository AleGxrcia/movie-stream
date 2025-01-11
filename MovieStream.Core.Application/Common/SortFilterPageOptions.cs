using MovieStream.Core.Application.Enums;

namespace MovieStream.Core.Application.Common
{
    public class SortFilterPageOptions
    {
        public const int DefaultPageSize = 10;

        private int _pageNum = 1;
        private int _pageSize = DefaultPageSize;

        private IReadOnlyList<int> PageSizes = new List<int> { 5, DefaultPageSize, 20, 50, 100, 500, 1000 };

        public OrderByOptions OrderByOptions { get; set; }
        public FilterBy FilterBy { get; set; }
        public string FilterValue { get; set; }

        public int PageNum
        {
            get => _pageNum;
            set => _pageNum = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = PageSizes.Contains(value) ? value : DefaultPageSize;
        }

        public int NumPages { get; private set; }
        public string PrevCheckState { get; set; }

        public void SetupRestOfDto<T>(IQueryable<T> query)
        {
            int totalItems = query.Count();
            NumPages = (int)Math.Ceiling((double)totalItems / PageSize);

            PageNum = Math.Min(Math.Max(1, PageNum), NumPages);

            var newCheckState = GenerateCheckState();

            if (PrevCheckState != newCheckState) PageNum = 1;

            PrevCheckState = newCheckState;
        }


        private string GenerateCheckState()
        {
            return $"{(int)FilterBy},{FilterValue},{PageSize},{NumPages}";
        }
    }
}
