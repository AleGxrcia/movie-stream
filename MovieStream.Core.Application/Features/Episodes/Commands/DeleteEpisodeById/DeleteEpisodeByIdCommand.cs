using MediatR;
using MovieStream.Core.Application.Interfaces.Repositories;

namespace MovieStream.Core.Application.Features.Episodes.Commands.DeleteEpisodeById
{
    public class DeleteEpisodeByIdCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class DeleteEpisodeByIdCommandHandler : IRequestHandler<DeleteEpisodeByIdCommand, int>
    {
        private readonly IEpisodeRepository _episodeRepository;

        public DeleteEpisodeByIdCommandHandler(IEpisodeRepository episodeRepository)
        {
            _episodeRepository = episodeRepository;
        }

        public async Task<int> Handle(DeleteEpisodeByIdCommand command, CancellationToken cancellationToken)
        {
            var episode = await _episodeRepository.GetByIdAsync(command.Id);

            if (episode == null) throw new Exception("Episode not found.");

            await _episodeRepository.DeleteAsync(episode);

            return episode.Id;
        }
    }
}
