using MediatR;
using MovieStream.Core.Application.Interfaces.Repositories;

namespace MovieStream.Core.Application.Features.Movies.Commands.DeleteMovieById
{
    public class DeleteMovieByIdCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class DeleteMovieByIdCommandHandler : IRequestHandler<DeleteMovieByIdCommand, int>
    {
        private readonly IMovieRepository _movieRepository;

        public DeleteMovieByIdCommandHandler(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<int> Handle(DeleteMovieByIdCommand command, CancellationToken cancellationToken)
        {
            var movie = await _movieRepository.GetByIdAsync(command.Id);

            if (movie == null) throw new Exception("Movie not found.");

            await _movieRepository.DeleteAsync(movie);

            return movie.Id;
        }
    }
}
