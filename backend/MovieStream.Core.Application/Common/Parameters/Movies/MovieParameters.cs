using MovieStream.Core.Application.Common.Parameters.Base;

namespace MovieStream.Core.Application.Common.Parameters.Movies
{
    public class MovieParameters : RequestParameters
    {
        public MovieParameters()
        {
            OrderBy = "title";
        }

        public int? MinReleaseYear { get; set; }
        public int? MaxReleaseYear { get; set; }

        public int? GenreId { get; set; }
        public string? SearchTerm { get; set; }
    }
}
