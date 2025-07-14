using AutoMapper;
using MediatR;
using MovieStream.Core.Application.Common.Parameters.Movies;
using MovieStream.Core.Application.DTOs.Movie;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using System.Net;

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
            var pagedMoviesDto = await GetAllDtoWithFilters(filters);

            if (pagedMoviesDto == null || pagedMoviesDto.Count == 0)
            {
                throw new ApiException("Movies not found.", (int)HttpStatusCode.NotFound);
            }

            return new Response<PagedList<MovieDto>>(pagedMoviesDto);
        }

        private async Task<PagedList<MovieDto>> GetAllDtoWithFilters(MovieParameters filters)
        {
            var movieList = await _movieRepository.GetAllWithFilters(filters);
            var movieDtoList = _mapper.Map<List<MovieDto>>(movieList);

            return new PagedList<MovieDto>(movieDtoList, movieList.MetaData.TotalCount, filters.PageNumber, filters.PageSize);
        }
    }
}
