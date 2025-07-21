using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using MovieStream.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace MovieStream.Core.Application.Features.Movies.Commands.UpdateMovie
{
    public class UpdateMovieCommand : IRequest<Response<MovieUpdateResponse>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan Runtime { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ProductionCompanyId { get; set; }
        public List<int> GenreIds { get; set; }


        [SwaggerIgnore]
        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
    }

    public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, Response<MovieUpdateResponse>>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public UpdateMovieCommandHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<Response<MovieUpdateResponse>> Handle(UpdateMovieCommand command, CancellationToken cancellationToken)
        {
            var movie = await _movieRepository.GetByIdAsync(command.Id);

            if (movie == null) throw new ApiException("Movie not found.", (int)HttpStatusCode.NotFound);

            movie = _mapper.Map<Movie>(command);
            await _movieRepository.UpdateAsync(movie, movie.Id);

            var movieResponse = _mapper.Map<MovieUpdateResponse>(movie);
            return new Response<MovieUpdateResponse>(movieResponse);
        }
    }
}
