using AutoMapper;
using MediatR;
using MovieStream.Core.Application.Common.Parameters.Base;
using MovieStream.Core.Application.DTOs.Episode;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;

namespace MovieStream.Core.Application.Features.Episodes.Queries.GetAllEpisodes
{
    public class GetAllEpisodesQuery : IRequest<Response<PagedList<EpisodeDto>>>
    {
        public RequestParameters Parameters { get; set; }
    }

    public class GetAllEpisodesQueryHandler : IRequestHandler<GetAllEpisodesQuery, Response<PagedList<EpisodeDto>>>
    {
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IMapper _mapper;

        public GetAllEpisodesQueryHandler(IEpisodeRepository episodeRepository, IMapper mapper)
        {
            _episodeRepository = episodeRepository;
            _mapper = mapper;
        }

        public async Task<Response<PagedList<EpisodeDto>>> Handle(GetAllEpisodesQuery request, CancellationToken cancellationToken)
        {
            var filters = request.Parameters;
            var episodesPagedResponse = await GetAllDtoWithFilters(filters);

            if (episodesPagedResponse == null || episodesPagedResponse.Data.Any()) throw new Exception("Episodes not found.");

            return episodesPagedResponse;
        }

        private async Task<PagedList<EpisodeDto>> GetAllDtoWithFilters(GetAllEpisodesParameters filters)
        {
            var (tvSeriesList, totalCount) = await _episodeRepository.GetAllWithFilters(filters);
            var tvSerieDtoList = _mapper.Map<List<EpisodeDto>>(tvSeriesList);

            return new PagedResponse<EpisodeDto>(tvSerieDtoList, filters.PageNumber, filters.PageSize, totalCount);
        }
    }
}