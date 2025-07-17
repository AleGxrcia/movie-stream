using AutoMapper;
using MediatR;
using MovieStream.Core.Application.DTOs.Episode;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using System.Net;

namespace MovieStream.Core.Application.Features.Episodes.Queries.GetEpisodeDetailsById
{
    public class GetEpisodeDetailsByIdQuery : IRequest<Response<EpisodeDto>>
    {
        public int Id { get; set; }
    }

    public class GetEpisodeDetailsByIdQueryHandler : IRequestHandler<GetEpisodeDetailsByIdQuery, Response<EpisodeDto>>
    {
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IMapper _mapper;

        public GetEpisodeDetailsByIdQueryHandler(IEpisodeRepository episodeRepository, IMapper mapper)
        {
            _episodeRepository = episodeRepository;
            _mapper = mapper;
        }

        public async Task<Response<EpisodeDto>> Handle(GetEpisodeDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var episodeDto = await GetByIdDto(request.Id);
            if (episodeDto == null) throw new ApiException("Episode not found.", (int)HttpStatusCode.NotFound);
            return new Response<EpisodeDto>(episodeDto);
        }

        private async Task<EpisodeDto> GetByIdDto(int id)
        {
            var episode = await _episodeRepository.GetByIdAsync(id);
            return _mapper.Map<EpisodeDto>(episode);
        }
    }
}
