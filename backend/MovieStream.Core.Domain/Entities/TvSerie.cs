using MovieStream.Core.Domain.Common;

namespace MovieStream.Core.Domain.Entities
{
    public class TvSerie : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ProductionCompanyId { get; set; }

        // Navigation properties
        public ICollection<Season> Seasons { get; set; } = new List<Season>();
        public ICollection<Genre>? Genres { get; set; }
        public ProductionCompany? ProductionCompany { get; set; }
    }
}
