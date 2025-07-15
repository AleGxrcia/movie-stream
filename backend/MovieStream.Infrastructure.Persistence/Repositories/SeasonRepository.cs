using Microsoft.EntityFrameworkCore;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Domain.Entities;
using MovieStream.Infrastructure.Persistence.Contexts;

namespace MovieStream.Infrastructure.Persistence.Repositories
{
    public class SeasonRepository : GenericRepository<Season>, ISeasonRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SeasonRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Season?> GetByIdWithInclude(int id)
        {
            return await _dbContext.Seasons
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
