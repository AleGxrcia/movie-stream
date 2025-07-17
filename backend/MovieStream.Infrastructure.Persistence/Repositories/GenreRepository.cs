using Microsoft.EntityFrameworkCore;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Domain.Entities;
using MovieStream.Infrastructure.Persistence.Contexts;

namespace MovieStream.Infrastructure.Persistence.Repositories
{
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public GenreRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Genre>> GetByIdsAsync(List<int> ids)
        {
            return await _dbContext.Set<Genre>().Where(g => ids.Contains(g.Id)).ToListAsync();
        }
    }
}
