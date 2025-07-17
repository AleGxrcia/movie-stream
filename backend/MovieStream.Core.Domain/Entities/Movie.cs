using MovieStream.Core.Domain.Common;

namespace MovieStream.Core.Domain.Entities
{
    public class Movie : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public TimeSpan Runtime { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ProductionCompanyId { get; set; }

        // Navigation properties
        public ICollection<Genre>? Genres { get; set; }
        public ProductionCompany? ProductionCompany { get; set; }
    }
}
