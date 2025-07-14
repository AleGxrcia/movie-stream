using AutoMapper;
using MediatR;
using MovieStream.Core.Application.DTOs.Movie;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using System.Net;

namespace MovieStream.Core.Application.Features.Movies.Queries.GetMovieById
{
    public class GetMovieByIdQuery : IRequest<Response<MovieDto>>
    {
        public int Id { get; set; }
    }

    public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, Response<MovieDto>>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public GetMovieByIdQueryHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<Response<MovieDto>> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var movieDto = await GetByIdDto(request.Id);
            if (movieDto == null) throw new ApiException("Movie not found.", (int)HttpStatusCode.NotFound);

            return new Response<MovieDto>(movieDto);
        }

        private async Task<MovieDto> GetByIdDto(int id)
        {
            var movie = await _movieRepository.GetByIdWithInclude(id);
            return _mapper.Map<MovieDto>(movie);
        }
    }
}
