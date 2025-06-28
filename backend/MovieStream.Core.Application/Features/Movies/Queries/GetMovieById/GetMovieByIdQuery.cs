using AutoMapper;
using MediatR;
using MovieStream.Core.Application.DTOs.Movie;
using MovieStream.Core.Application.Interfaces.Repositories;

namespace MovieStream.Core.Application.Features.Movies.Queries.GetMovieById
{
    public class GetMovieByIdQuery : IRequest<MovieDto>
    {
        public int Id { get; set; }
    }

    public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, MovieDto>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public GetMovieByIdQueryHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<MovieDto> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var movie = await GetByIdDto(request.Id);
            if (movie == null) throw new Exception("Movie not found.");
            return movie;
        }

        private async Task<MovieDto> GetByIdDto(int id)
        {
            var movie = await _movieRepository.GetByIdWithInclude(id);
            return _mapper.Map<MovieDto>(movie);
        }
    }
}
