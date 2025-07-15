using Microsoft.EntityFrameworkCore;
using MovieStream.Core.Application.Common.Parameters.Base;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using MovieStream.Infrastructure.Persistence.Contexts;

namespace MovieStream.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : class
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<Entity> AddAsync(Entity entity)
        {
            await _dbContext.Set<Entity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(Entity entity, int id)
        {
            var entry = await _dbContext.Set<Entity>().FindAsync(id);
            _dbContext.Entry(entry).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Entity entity)
        {
            _dbContext.Set<Entity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<List<Entity>> GetAllAsync()
        {
            return await _dbContext.Set<Entity>().ToListAsync();
        }

        public virtual async Task<PagedList<Entity>> GetAllWithFilters(RequestParameters parameters)
        {
            var query = _dbContext.Set<Entity>()
                .AsQueryable()
                .AsNoTracking();

            var totalCount = await query.CountAsync();

            var itemsForCurrentPage = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return PagedList<Entity>.ToPagedList(itemsForCurrentPage, totalCount, parameters.PageNumber, parameters.PageSize);
        }

        public virtual async Task<List<Entity>> GetAllWithIncludeAsync(List<string> properties)
        {
            var query = _dbContext.Set<Entity>().AsQueryable();

            foreach (string property in properties)
            {
                query = query.Include(property);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<Entity> GetByIdAsync(int id)
        {
            return await _dbContext.Set<Entity>().FindAsync(id);
        }
    }
}
