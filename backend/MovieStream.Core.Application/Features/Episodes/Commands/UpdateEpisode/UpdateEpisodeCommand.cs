using AutoMapper;
using MediatR;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using MovieStream.Core.Domain.Entities;
using System.Net;

namespace MovieStream.Core.Application.Features.Episodes.Commands.UpdateEpisode
{
    public class UpdateEpisodeCommand : IRequest<Response<EpisodeUpdateResponse>>
    {
        public int Id { get; set; }
        public int EpisodeNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan Runtime { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string StreamingUrl { get; set; }
        public int SeasonId { get; set; }
    }

    public class UpdateEpisodeCommandHandler : IRequestHandler<UpdateEpisodeCommand, Response<EpisodeUpdateResponse>>
    {
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IMapper _mapper;

        public UpdateEpisodeCommandHandler(IEpisodeRepository episodeRepository, IMapper mapper)
        {
            _episodeRepository = episodeRepository;
            _mapper = mapper;
        }

        public async Task<Response<EpisodeUpdateResponse>> Handle(UpdateEpisodeCommand command, CancellationToken cancellationToken)
        {
            var episode = await _episodeRepository.GetByIdAsync(command.Id);

            if (episode == null) throw new ApiException("Episode not found.", (int)HttpStatusCode.NotFound);

            episode = _mapper.Map<Episode>(command);

            await _episodeRepository.UpdateAsync(episode, episode.Id);

            var episodeResponse = _mapper.Map<EpisodeUpdateResponse>(episode);

            return new Response<EpisodeUpdateResponse>(episodeResponse);
        }
    }
}
