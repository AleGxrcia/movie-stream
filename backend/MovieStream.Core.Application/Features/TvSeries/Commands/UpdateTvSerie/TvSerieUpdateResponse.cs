using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace MovieStream.Core.Application.Features.TvSeries.Commands.UpdateTvSerie
{
    public class TvSerieUpdateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ProductionCompanyId { get; set; }
        public List<int> GenreIds { get; set; }


        [JsonIgnore]
        public string? ImageUrl { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
