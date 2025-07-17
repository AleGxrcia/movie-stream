using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.Interfaces.Repositories
{
    public interface IGenreRepository : IGenericRepository<Genre>
    {
        Task<List<Genre>> GetByIdsAsync(List<int> ids);
    }
}
