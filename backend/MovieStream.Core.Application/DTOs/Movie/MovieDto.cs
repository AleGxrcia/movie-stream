using MovieStream.Core.Application.DTOs.Genre;
using MovieStream.Core.Application.DTOs.ProductionCompany;

namespace MovieStream.Core.Application.DTOs.Movie
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public TimeSpan Runtime { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ProductionCompanyId { get; set; }
        public ICollection<GenreDto>? Genres { get; set; }
        public ProductionCompanyDto? ProductionCompany { get; set; }
    }
}
