using MovieStream.Core.Application.Common.Parameters.Base;

namespace MovieStream.Core.Application.Common.Parameters.TvSeries
{
    public class TvSerieParameters : RequestParameters
    {
        public TvSerieParameters()
        {
            OrderBy = "name";
        }

        public int? MinReleaseYear { get; set; }
        public int? MaxReleaseYear { get; set; }
        public int? GenreId { get; set; }
    }
}
