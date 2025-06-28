using AutoMapper;
using MediatR;
using MovieStream.Core.Application.DTOs.Episode;
using MovieStream.Core.Application.Interfaces.Repositories;

namespace MovieStream.Core.Application.Features.Episodes.Queries.GetEpisodeDetailsById
{
    public class GetEpisodeDetailsByIdQuery : IRequest<EpisodeDto>
    {
        public int Id { get; set; }
    }

    public class GetEpisodeDetailsByIdQueryHandler : IRequestHandler<GetEpisodeDetailsByIdQuery, EpisodeDto>
    {
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IMapper _mapper;

        public GetEpisodeDetailsByIdQueryHandler(IEpisodeRepository episodeRepository, IMapper mapper)
        {
            _episodeRepository = episodeRepository;
            _mapper = mapper;
        }

        public async Task<EpisodeDto> Handle(GetEpisodeDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var episode = await GetByIdDto(request.Id);
            if (episode == null) throw new Exception("Episode not found.");
            return episode;
        }

        private async Task<EpisodeDto> GetByIdDto(int id)
        {
            var episode = await _episodeRepository.GetByIdAsync(id);
            return _mapper.Map<EpisodeDto>(episode);
        }
    }
}
