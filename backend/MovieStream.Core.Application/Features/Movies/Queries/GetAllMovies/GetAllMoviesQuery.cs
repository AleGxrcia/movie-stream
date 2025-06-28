using AutoMapper;
using MediatR;
using MovieStream.Core.Application.DTOs.Movie;
using MovieStream.Core.Application.Interfaces.Repositories;

namespace MovieStream.Core.Application.Features.Movies.Queries.GetAllMovies
{
    public class GetAllMoviesQuery : IRequest<PagedResponse<MovieDto>>
    {
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public string? FilterBy { get; set; }
        public string? FilterValue { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllMoviesQueryHandler : IRequestHandler<GetAllMoviesQuery, PagedResponse<MovieDto>>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public GetAllMoviesQueryHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<MovieDto>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
        {
            var filters = _mapper.Map<GetAllMoviesParameters>(request);
            var moviesPagedResponse = await GetAllDtoWithFilters(filters);

            if (moviesPagedResponse != null && (moviesPagedResponse.Data == null || !moviesPagedResponse.Data.Any()))
            {
                return new PagedResponse<MovieDto>(new List<MovieDto>(), filters.PageNumber, filters.PageSize, 0);
            }

            if (moviesPagedResponse == null)
            {
                return new PagedResponse<MovieDto>(new List<MovieDto>(), filters.PageNumber, filters.PageSize, 0);
            }

            return moviesPagedResponse;
        }

        private async Task<PagedResponse<MovieDto>> GetAllDtoWithFilters(GetAllMoviesParameters filters)
        {
            var (movieList, totalCount) = await _movieRepository.GetAllWithFilters(filters);
            var movieDtoList = _mapper.Map<List<MovieDto>>(movieList);

            return new PagedResponse<MovieDto>(movieDtoList, filters.PageNumber, filters.PageSize, totalCount);
        }
    }
}
