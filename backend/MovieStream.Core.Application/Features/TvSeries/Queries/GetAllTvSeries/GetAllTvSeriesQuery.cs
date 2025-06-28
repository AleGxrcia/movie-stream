using AutoMapper;
using MediatR;
using MovieStream.Core.Application.DTOs.TvSerie;
using MovieStream.Core.Application.Interfaces.Repositories;

namespace MovieStream.Core.Application.Features.TvSeries.Queries.GetAllTvSeries
{
    public class GetAllTvSeriesQuery : IRequest<PagedResponse<TvSerieDto>>
    {
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public string? FilterBy { get; set; }
        public string? FilterValue { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllTvSeriesQueryHandler : IRequestHandler<GetAllTvSeriesQuery, PagedResponse<TvSerieDto>>
    {
        private readonly ITvSerieRepository _tvSerieRepository;
        private readonly IMapper _mapper;

        public GetAllTvSeriesQueryHandler(ITvSerieRepository tvSerieRepository, IMapper mapper)
        {
            _tvSerieRepository = tvSerieRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<TvSerieDto>> Handle(GetAllTvSeriesQuery request, CancellationToken cancellationToken)
        {
            var filters = _mapper.Map<GetAllTvSeriesParameters>(request);
            var tvSeriesPagedResponse = await GetAllDtoWithFilters(filters);

            if (tvSeriesPagedResponse == null || !tvSeriesPagedResponse.Data.Any())
                throw new Exception("TvSeries not found.");

            return tvSeriesPagedResponse;
        }

        private async Task<PagedResponse<TvSerieDto>> GetAllDtoWithFilters(GetAllTvSeriesParameters filters)
        {
            var (tvSeriesList, totalCount) = await _tvSerieRepository.GetAllWithFilters(filters);
            var tvSerieDtoList = _mapper.Map<List<TvSerieDto>>(tvSeriesList);

            return new PagedResponse<TvSerieDto>(tvSerieDtoList, filters.PageNumber, filters.PageSize, totalCount);
        }
    }
}
