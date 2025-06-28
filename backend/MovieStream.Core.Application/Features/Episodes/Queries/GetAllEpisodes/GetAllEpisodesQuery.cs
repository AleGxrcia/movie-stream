using AutoMapper;
using MediatR;
using MovieStream.Core.Application.DTOs.Episode;
using MovieStream.Core.Application.Interfaces.Repositories;

namespace MovieStream.Core.Application.Features.Episodes.Queries.GetAllEpisodes
{
    public class GetAllEpisodesQuery : IRequest<PagedResponse<EpisodeDto>>
    {
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public string? FilterBy { get; set; }
        public string? FilterValue { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllEpisodesQueryHandler : IRequestHandler<GetAllEpisodesQuery, PagedResponse<EpisodeDto>>
    {
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IMapper _mapper;

        public GetAllEpisodesQueryHandler(IEpisodeRepository episodeRepository, IMapper mapper)
        {
            _episodeRepository = episodeRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<EpisodeDto>> Handle(GetAllEpisodesQuery request, CancellationToken cancellationToken)
        {
            var filters = _mapper.Map<GetAllEpisodesParameters>(request);
            var episodesPagedResponse = await GetAllDtoWithFilters(filters);

            if (episodesPagedResponse == null || episodesPagedResponse.Data.Any()) throw new Exception("Episodes not found.");

            return episodesPagedResponse;
        }

        private async Task<PagedResponse<EpisodeDto>> GetAllDtoWithFilters(GetAllEpisodesParameters filters)
        {
            var (tvSeriesList, totalCount) = await _episodeRepository.GetAllWithFilters(filters);
            var tvSerieDtoList = _mapper.Map<List<EpisodeDto>>(tvSeriesList);

            return new PagedResponse<EpisodeDto>(tvSerieDtoList, filters.PageNumber, filters.PageSize, totalCount);
        }
    }
}