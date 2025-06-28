using AutoMapper;
using MediatR;
using MovieStream.Core.Application.DTOs.Season;
using MovieStream.Core.Application.Interfaces.Repositories;

namespace MovieStream.Core.Application.Features.Seasons.Queries.GetAllSeasons
{
    public class GetAllSeasonsQuery : IRequest<PagedResponse<SeasonDto>>
    {
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public string? FilterBy { get; set; }
        public string? FilterValue { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllSeasonsQueryHandler : IRequestHandler<GetAllSeasonsQuery, PagedResponse<SeasonDto>>
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IMapper _mapper;

        public GetAllSeasonsQueryHandler(ISeasonRepository seasonRepository, IMapper mapper)
        {
            _seasonRepository = seasonRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<SeasonDto>> Handle(GetAllSeasonsQuery request, CancellationToken cancellationToken)
        {
            var filters = _mapper.Map<GetAllSeasonsParameters>(request);
            var seasonsPagedResponse = await GetAllDtoWithFilters(filters);

            if (seasonsPagedResponse == null || seasonsPagedResponse.Data.Any()) throw new Exception("Seasons not found.");

            return seasonsPagedResponse;
        }

        private async Task<PagedResponse<SeasonDto>> GetAllDtoWithFilters(GetAllSeasonsParameters filters)
        {
            var (seasonList, totalCount) = await _seasonRepository.GetAllWithFilters(filters);
            var seasonDtoList = _mapper.Map<List<SeasonDto>>(seasonList);

            return new PagedResponse<SeasonDto>(seasonDtoList, filters.PageNumber, filters.PageSize, totalCount);
        }
    }
}