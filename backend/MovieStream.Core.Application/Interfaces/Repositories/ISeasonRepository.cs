using MovieStream.Core.Application.Features.Seasons.Queries.GetAllSeasons;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.Interfaces.Repositories
{
    public interface ISeasonRepository : IGenericRepository<Season>
    {
        Task<Season?> GetByIdWithInclude(int id);
    }
}
