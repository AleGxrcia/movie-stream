using MovieStream.Core.Domain.Common;

namespace MovieStream.Core.Domain.Entities
{
    public class Season : AuditableBaseEntity
    {
        public int SeasonNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int TvSerieId { get; set; }

        // Navigation properties
        public TvSerie? TvSerie { get; set; }
        public ICollection<Episode> Episodes { get; set; } = new List<Episode>();
    }
}
