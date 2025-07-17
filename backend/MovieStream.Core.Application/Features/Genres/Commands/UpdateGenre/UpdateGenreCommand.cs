using AutoMapper;
using MediatR;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using MovieStream.Core.Domain.Entities;
using System.Net;

namespace MovieStream.Core.Application.Features.Genres.Commands.UpdateGenre
{
    public class UpdateGenreCommand : IRequest<Response<GenreUpdateResponse>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, Response<GenreUpdateResponse>>
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public UpdateGenreCommandHandler(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<Response<GenreUpdateResponse>> Handle(UpdateGenreCommand command, CancellationToken cancellationToken)
        {
            var genre = await _genreRepository.GetByIdAsync(command.Id);

            if (genre == null) throw new ApiException("Genre not found.", (int)HttpStatusCode.NotFound);

            genre = _mapper.Map<Genre>(command);

            await _genreRepository.UpdateAsync(genre, genre.Id);

            var genreResponse = _mapper.Map<GenreUpdateResponse>(genre);

            return new Response<GenreUpdateResponse>(genreResponse);
        }
    }
}
