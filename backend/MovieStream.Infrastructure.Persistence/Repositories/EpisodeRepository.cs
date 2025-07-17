using Microsoft.EntityFrameworkCore;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Domain.Entities;
using MovieStream.Infrastructure.Persistence.Contexts;

namespace MovieStream.Infrastructure.Persistence.Repositories
{
    public class EpisodeRepository : GenericRepository<Episode>, IEpisodeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EpisodeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Episode?> GetByIdWithInclude(int id)
        {
            return await _dbContext.Episodes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
