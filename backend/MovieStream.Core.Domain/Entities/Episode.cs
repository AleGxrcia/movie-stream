using MovieStream.Core.Domain.Common;

namespace MovieStream.Core.Domain.Entities
{
    public class Episode : AuditableBaseEntity
    {
        public int EpisodeNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan Runtime { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string StreamingUrl { get; set; }
        public int SeasonId { get; set; }

        // Navigation properties
        public Season? Season { get; set; }
    }
}
