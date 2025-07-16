using AutoMapper;
using MediatR;
using MovieStream.Core.Application.DTOs.Genre;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using System.Net;

namespace MovieStream.Core.Application.Features.Genres.Queries.GetGenreById
{
    public class GetGenreByIdQuery : IRequest<Response<GenreDto>>
    {
        public int Id { get; set; }
    }

    public class GetGenreByIdQueryHandler : IRequestHandler<GetGenreByIdQuery, Response<GenreDto>>
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GetGenreByIdQueryHandler(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<Response<GenreDto>> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
        {
            var genreDto = await GetByIdDto(request.Id);
            if (genreDto == null) throw new ApiException("Genre not found.", (int)HttpStatusCode.NotFound);
            return new Response<GenreDto>(genreDto);
        }

        private async Task<GenreDto> GetByIdDto(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);
            return _mapper.Map<GenreDto>(genre);
        }
    }
}
