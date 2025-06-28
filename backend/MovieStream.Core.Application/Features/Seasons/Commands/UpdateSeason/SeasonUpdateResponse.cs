namespace MovieStream.Core.Application.Features.Seasons.Commands.UpdateSeason
{
    public class SeasonUpdateResponse
    {
        public int Id { get; set; }
        public int SeasonNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int TvSerieId { get; set; }
    }
}
