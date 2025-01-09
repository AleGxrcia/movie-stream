using MovieStream.Core.Application.DTOs.Genre;
using MovieStream.Core.Application.DTOs.ProductionCompany;
using MovieStream.Core.Application.DTOs.Season;

namespace MovieStream.Core.Application.DTOs.TvSerie
{
    public class TvSerieDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime ReleaseDate { get; set; }
        public ICollection<SeasonDto>? Seasons { get; set; }
        public ICollection<GenreDto>? Genres { get; set; }
        public ProductionCompanyDto? ProductionCompany { get; set; }
    }
}
