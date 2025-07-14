using MediatR;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using System.Net;

namespace MovieStream.Core.Application.Features.Movies.Commands.DeleteMovieById
{
    public class DeleteMovieByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteMovieByIdCommandHandler : IRequestHandler<DeleteMovieByIdCommand, Response<int>>
    {
        private readonly IMovieRepository _movieRepository;

        public DeleteMovieByIdCommandHandler(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<Response<int>> Handle(DeleteMovieByIdCommand command, CancellationToken cancellationToken)
        {
            var movie = await _movieRepository.GetByIdAsync(command.Id);

            if (movie == null) throw new ApiException("Movie not found.", (int)HttpStatusCode.NotFound);

            await _movieRepository.DeleteAsync(movie);
            return new Response<int>(movie.Id);
        }
    }
}
