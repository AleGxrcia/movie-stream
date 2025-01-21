using AutoMapper;
using MediatR;
using MovieStream.Core.Application.DTOs.Genre;
using MovieStream.Core.Application.Interfaces.Repositories;

namespace MovieStream.Core.Application.Features.Genres.Queries.GetAllGenres
{
    public class GetAllGenresQuery : IRequest<IList<GenreDto>>
    {
    }

    public class GetAllGenresQueryHandler : IRequestHandler<GetAllGenresQuery, IList<GenreDto>>
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GetAllGenresQueryHandler(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<IList<GenreDto>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
        {
            var genreList = await GetAllDtoWithFilters();
            if (genreList == null || genreList.Count == 0) throw new Exception("Genres not found.");
            return genreList;
        }

        private async Task<List<GenreDto>> GetAllDtoWithFilters()
        {
            var genreList = await _genreRepository.GetAllAsync();
            return _mapper.Map<List<GenreDto>>(genreList);
        }
    }
}
