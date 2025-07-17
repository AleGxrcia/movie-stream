using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using MovieStream.Core.Application.DTOs.Genre;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using MovieStream.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MovieStream.Core.Application.Features.Movies.Commands.CreateMovie
{
    public class CreateMovieCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan Runtime { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ProductionCompanyId { get; set; }
        public List<int> GenreIds { get; set; }


        [SwaggerIgnore]
        public string? ImagePath { get; set; }
        public IFormFile ImageFile { get; set; }
    }

    public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, Response<int>>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public CreateMovieCommandHandler(IMovieRepository movieRepository, IGenreRepository genreRepository,
            IMapper mapper)
        {
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateMovieCommand command, CancellationToken cancellationToken)
        {
            var movie = _mapper.Map<Movie>(command);
            var genres = await _genreRepository.GetByIdsAsync(command.GenreIds);

            movie.Genres = genres;

            movie = await _movieRepository.AddAsync(movie);
            return new Response<int>(movie.Id);
        }
    }
}
