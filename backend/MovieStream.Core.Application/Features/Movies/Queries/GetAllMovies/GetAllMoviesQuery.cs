using AutoMapper;
using MediatR;
using MovieStream.Core.Application.Common.Parameters.Movies;
using MovieStream.Core.Application.DTOs.Movie;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;

namespace MovieStream.Core.Application.Features.Movies.Queries.GetAllMovies
{
    public class GetAllMoviesQuery : IRequest<Response<PagedList<MovieDto>>>
    {
        public MovieParameters Parameters { get; set; }
    }

    public class GetAllMoviesQueryHandler : IRequestHandler<GetAllMoviesQuery, Response<PagedList<MovieDto>>>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public GetAllMoviesQueryHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<Response<PagedList<MovieDto>>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
        {
            var filters = request.Parameters;
            var moviesPagedResponse = await GetAllDtoWithFilters(filters);

            if (moviesPagedResponse != null && (moviesPagedResponse.Data == null || !moviesPagedResponse.Data.Any()))
            {
                return new Response<PagedList<MovieDto>>(
                    new PagedList<MovieDto>(new List<MovieDto>(), 0, filters.PageNumber, filters.PageSize),
                    "No movies found");
            }

            if (moviesPagedResponse == null)
            {
                return new Response<PagedList<MovieDto>>(
                    new PagedList<MovieDto>(new List<MovieDto>(), 0, filters.PageNumber, filters.PageSize),
                    "Error retrieving movies");
            }

            return new Response<PagedList<MovieDto>>(moviesPagedResponse);
        }

        private async Task<PagedList<MovieDto>> GetAllDtoWithFilters(MovieParameters filters)
        {
            var movieList = await _movieRepository.GetAllWithFilters(filters);
            var movieDtoList = _mapper.Map<List<MovieDto>>(movieList);

            return new PagedList<MovieDto>(movieDtoList, ,, totalCount);
        }
    }
}
