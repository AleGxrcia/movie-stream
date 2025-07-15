using MovieStream.Core.Application.Common.Parameters.Base;
using MovieStream.Core.Application.Wrappers;

namespace MovieStream.Core.Application.Interfaces.Repositories
{
    public interface IGenericRepository<Entity> where Entity : class
    {
        Task<Entity> AddAsync(Entity entity);
        Task UpdateAsync(Entity entity, int id);
        Task DeleteAsync(Entity entity);
        Task<List<Entity>> GetAllAsync();
        Task<PagedList<Entity>> GetAllWithFilters(RequestParameters parameters)
        Task<List<Entity>> GetAllWithIncludeAsync(List<string> properties);
        Task<Entity> GetByIdAsync(int id);
    }
}