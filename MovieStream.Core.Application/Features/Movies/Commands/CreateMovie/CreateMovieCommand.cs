using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Domain.Entities;
using System.Text.Json.Serialization;

namespace MovieStream.Core.Application.Features.Movies.Commands.CreateMovie
{
    public class CreateMovieCommand : IRequest<int>
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan Runtime { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ProductionCompanyId { get; set; }
        public List<int> GenreIds { get; set; }


        [JsonIgnore]
        public string? ImagePath { get; set; }
        public IFormFile ImageFile { get; set; }
    }

    public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, int>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public CreateMovieCommandHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateMovieCommand command, CancellationToken cancellationToken)
        {
            var movie = _mapper.Map<Movie>(command);
            movie = await _movieRepository.AddAsync(movie);
            return movie.Id;
        }
    }
}
