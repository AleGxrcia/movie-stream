using MovieStream.Core.Application.DTOs.Episode;

namespace MovieStream.Core.Application.DTOs.Season
{
    public class SeasonDto
    {
        public int Id { get; set; }
        public int SeasonNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public ICollection<EpisodeDto>? Episodes { get; set; }
    }
}
