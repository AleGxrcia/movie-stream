using MovieStream.Core.Application.Features.Episodes.Queries.GetAllEpisodes;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.Interfaces.Repositories
{
    public interface IEpisodeRepository : IGenericRepository<Episode>
    {
        Task<Episode?> GetByIdWithInclude(int id);
    }
}
