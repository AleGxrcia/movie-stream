using MediatR;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using System.Net;

namespace MovieStream.Core.Application.Features.Episodes.Commands.DeleteEpisodeById
{
    public class DeleteEpisodeByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteEpisodeByIdCommandHandler : IRequestHandler<DeleteEpisodeByIdCommand, Response<int>>
    {
        private readonly IEpisodeRepository _episodeRepository;

        public DeleteEpisodeByIdCommandHandler(IEpisodeRepository episodeRepository)
        {
            _episodeRepository = episodeRepository;
        }

        public async Task<Response<int>> Handle(DeleteEpisodeByIdCommand command, CancellationToken cancellationToken)
        {
            var episode = await _episodeRepository.GetByIdAsync(command.Id);

            if (episode == null) throw new ApiException("Episode not found.", (int)HttpStatusCode.NotFound);

            await _episodeRepository.DeleteAsync(episode);

            return new Response<int>(episode.Id);
        }
    }
}
