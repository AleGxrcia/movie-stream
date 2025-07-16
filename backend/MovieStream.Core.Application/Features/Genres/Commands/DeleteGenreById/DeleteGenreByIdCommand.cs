using MediatR;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using System.Net;

namespace MovieStream.Core.Application.Features.Genres.Commands.DeleteGenreById
{
    public class DeleteGenreByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteGenreByIdCommandHandler : IRequestHandler<DeleteGenreByIdCommand, Response<int>>
    {
        private readonly IGenreRepository _genreRepository;

        public DeleteGenreByIdCommandHandler(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<Response<int>> Handle(DeleteGenreByIdCommand command, CancellationToken cancellationToken)
        {
            var genre = await _genreRepository.GetByIdAsync(command.Id);

            if (genre == null) throw new ApiException("Genre not found.", (int)HttpStatusCode.NotFound);

            await _genreRepository.DeleteAsync(genre);

            return new Response<int>(genre.Id);
        }
    }
}
