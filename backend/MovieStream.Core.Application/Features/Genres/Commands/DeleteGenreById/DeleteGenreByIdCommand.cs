using MediatR;
using MovieStream.Core.Application.Interfaces.Repositories;

namespace MovieStream.Core.Application.Features.Genres.Commands.DeleteGenreById
{
    public class DeleteGenreByIdCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class DeleteGenreByIdCommandHandler : IRequestHandler<DeleteGenreByIdCommand, int>
    {
        private readonly IGenreRepository _genreRepository;

        public DeleteGenreByIdCommandHandler(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<int> Handle(DeleteGenreByIdCommand command, CancellationToken cancellationToken)
        {
            var genre = await _genreRepository.GetByIdAsync(command.Id);

            if (genre == null) throw new Exception("Genre not found.");

            await _genreRepository.DeleteAsync(genre);

            return genre.Id;
        }
    }
}
