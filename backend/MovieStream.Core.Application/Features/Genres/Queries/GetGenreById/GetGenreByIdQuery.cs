using AutoMapper;
using MediatR;
using MovieStream.Core.Application.DTOs.Genre;
using MovieStream.Core.Application.Interfaces.Repositories;

namespace MovieStream.Core.Application.Features.Genres.Queries.GetGenreById
{
    public class GetGenreByIdQuery : IRequest<GenreDto>
    {
        public int Id { get; set; }
    }

    public class GetGenreByIdQueryHandler : IRequestHandler<GetGenreByIdQuery, GenreDto>
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GetGenreByIdQueryHandler(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<GenreDto> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
        {
            var genre = await GetByIdDto(request.Id);
            if (genre == null) throw new Exception("Genre not found.");
            return genre;
        }

        private async Task<GenreDto> GetByIdDto(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);
            return _mapper.Map<GenreDto>(genre);
        }
    }
}
