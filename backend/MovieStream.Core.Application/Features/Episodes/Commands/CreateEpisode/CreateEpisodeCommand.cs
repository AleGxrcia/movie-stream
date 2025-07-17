using AutoMapper;
using MediatR;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.Features.Episodes.Commands.CreateEpisode
{
    public class CreateEpisodeCommand : IRequest<Response<int>>
    {
        public int EpisodeNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan Runtime { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string StreamingUrl { get; set; }
        public int SeasonId { get; set; }
    }

    public class CreateEpisodeCommandHandler : IRequestHandler<CreateEpisodeCommand, Response<int>>
    {
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IMapper _mapper;

        public CreateEpisodeCommandHandler(IEpisodeRepository episodeRepository, IMapper mapper)
        {
            _episodeRepository = episodeRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateEpisodeCommand command, CancellationToken cancellationToken)
        {
            var episode = _mapper.Map<Episode>(command);
            episode = await _episodeRepository.AddAsync(episode);
            return new Response<int>(episode.Id);
        }
    }
}
