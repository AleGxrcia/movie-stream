using MovieStream.Core.Domain.Common;

namespace MovieStream.Core.Domain.Entities
{
    public class ProductionCompany : AuditableBaseEntity
    {
        public string Name { get; set; }

        // Navigation properties
        public ICollection<Movie>? Movies { get; set; }
        public ICollection<TvSerie>? TvSeries { get; set; }
    }
}
