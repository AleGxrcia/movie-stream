using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace MovieStream.Core.Application.DTOs.TvSerie
{
    public class SaveTvSerieDto
    {
        public int? Id { get; set; } 
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ProductionCompanyId { get; set; }
        public List<int> GenreIds { get; set; }


        [JsonIgnore]
        public string? ImagePath { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
