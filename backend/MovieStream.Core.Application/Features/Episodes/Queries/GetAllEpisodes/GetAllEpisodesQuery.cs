using AutoMapper;
using MediatR;
using MovieStream.Core.Application.Common.Parameters.Episodes;
using MovieStream.Core.Application.DTOs.Episode;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using System.Net;

namespace MovieStream.Core.Application.Features.Episodes.Queries.GetAllEpisodes
{
    public class GetAllEpisodesQuery : IRequest<Response<PagedList<EpisodeDto>>>
    {
        public EpisodeParameters Parameters { get; set; }
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
            var pagedEpisodesDto = await GetAllDtoWithFilters(filters);

            if (pagedEpisodesDto == null || pagedEpisodesDto.Count == 0)
            {
                throw new ApiException("Episodes not found.", (int)HttpStatusCode.NotFound);
            }

            return new Response<PagedList<EpisodeDto>>(pagedEpisodesDto, pagedEpisodesDto.MetaData);
        }

        private async Task<PagedList<EpisodeDto>> GetAllDtoWithFilters(EpisodeParameters parameters)
        {
            var episodeList = await _episodeRepository.GetAllWithFilters(parameters);
            var episodeDtoList = _mapper.Map<List<EpisodeDto>>(episodeList);

            return new PagedList<EpisodeDto>(episodeDtoList, episodeList.MetaData.TotalCount, parameters.PageNumber, parameters.PageSize);
        }
    }
}