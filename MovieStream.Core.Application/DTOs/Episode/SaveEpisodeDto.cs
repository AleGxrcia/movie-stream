namespace MovieStream.Core.Application.DTOs.Episode
{
    public class SaveEpisodeDto
    {
        public int? Id { get; set; }
        public int EpisodeNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan Runtime { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string StreamingUrl { get; set; }
        public int SeasonId { get; set; }
    }
}
