using AutoMapper;
using MediatR;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.Features.Movies.Commands.UpdateMovie
{
    public class UpdateMovieCommand : IRequest<MovieUpdateResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, MovieUpdateResponse>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public UpdateMovieCommandHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<MovieUpdateResponse> Handle(UpdateMovieCommand command, CancellationToken cancellationToken)
        {
            var movie = await _movieRepository.GetByIdAsync(command.Id);

            if (movie == null) throw new Exception("Movie not found.");

            movie = _mapper.Map<Movie>(command);

            await _movieRepository.UpdateAsync(movie, movie.Id);

            var movieResponse = _mapper.Map<MovieUpdateResponse>(movie);

            return movieResponse;
        }
    }
}
