using AutoMapper;
using MediatR;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.Features.Genres.Commands.UpdateGenre
{
    public class UpdateGenreCommand : IRequest<GenreUpdateResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, GenreUpdateResponse>
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public UpdateGenreCommandHandler(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<GenreUpdateResponse> Handle(UpdateGenreCommand command, CancellationToken cancellationToken)
        {
            var genre = await _genreRepository.GetByIdAsync(command.Id);

            if (genre == null) throw new Exception("Genre not found.");

            genre = _mapper.Map<Genre>(command);

            await _genreRepository.UpdateAsync(genre, genre.Id);

            var genreResponse = _mapper.Map<GenreUpdateResponse>(genre);

            return genreResponse;
        }
    }
}
