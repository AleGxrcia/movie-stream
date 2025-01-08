using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Domain.Entities;
using MovieStream.Infrastructure.Persistence.Contexts;

namespace MovieStream.Infrastructure.Persistence.Repositories
{
    public class ProductionCompanyRepository : GenericRepository<ProductionCompany>, IProductionCompanyRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductionCompanyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
