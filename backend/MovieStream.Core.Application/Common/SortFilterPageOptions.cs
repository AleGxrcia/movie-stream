using System.ComponentModel.DataAnnotations;

namespace MovieStream.Core.Application.Common
{
    public class SortFilterPageOptions<TSortByOptions, TFilterBy>
        where TSortByOptions : Enum
        where TFilterBy : Enum 
    {
        public const int DefaultPageSize = 10;

        private int _pageNum = 1;
        private int _pageSize = DefaultPageSize;

        public TSortByOptions? SortByOptions { get; set; }
        public TFilterBy? FilterBy { get; set; }
        public string? FilterValue { get; set; }

        public int PageNum
        {
            get => _pageNum;
            set => _pageNum = value < 1 ? 1 : value;
        }

        [Range(10, 50, ErrorMessage = "The page size must be between {1} and {2}.")]
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 50 ? 50 : value;
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
            return $"{(Enum)FilterBy},{FilterValue},{PageSize},{NumPages}";
        }
    }
}
