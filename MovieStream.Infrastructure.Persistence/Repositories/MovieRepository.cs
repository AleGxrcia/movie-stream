using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Domain.Entities;
using MovieStream.Infrastructure.Persistence.Contexts;

namespace MovieStream.Infrastructure.Persistence.Repositories
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MovieRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
