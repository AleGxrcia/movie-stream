using AutoMapper;
using MediatR;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.Features.Episodes.Commands.UpdateEpisode
{
    public class UpdateEpisodeCommand : IRequest<EpisodeUpdateResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateEpisodeCommandHandler : IRequestHandler<UpdateEpisodeCommand, EpisodeUpdateResponse>
    {
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IMapper _mapper;

        public UpdateEpisodeCommandHandler(IEpisodeRepository episodeRepository, IMapper mapper)
        {
            _episodeRepository = episodeRepository;
            _mapper = mapper;
        }

        public async Task<EpisodeUpdateResponse> Handle(UpdateEpisodeCommand command, CancellationToken cancellationToken)
        {
            var episode = await _episodeRepository.GetByIdAsync(command.Id);

            if (episode == null) throw new Exception("Episode not found.");

            episode = _mapper.Map<Episode>(command);

            await _episodeRepository.UpdateAsync(episode, episode.Id);

            var episodeResponse = _mapper.Map<EpisodeUpdateResponse>(episode);

            return episodeResponse;
        }
    }
}
