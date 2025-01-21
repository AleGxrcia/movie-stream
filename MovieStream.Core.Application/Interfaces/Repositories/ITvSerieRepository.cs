using MovieStream.Core.Application.Features.TvSeries.Queries.GetAllTvSeries;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.Interfaces.Repositories
{
    public interface ITvSerieRepository : IGenericRepository<TvSerie>
    {
        Task<(List<TvSerie>, int totalCount)> GetAllWithFilters(GetAllTvSeriesParameters filters);
        Task<TvSerie?> GetByIdWithInclude(int id);
    }
}
