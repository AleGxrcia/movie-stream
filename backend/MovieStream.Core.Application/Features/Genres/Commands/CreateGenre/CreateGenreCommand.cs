using AutoMapper;
using MediatR;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.Features.Genres.Commands.CreateGenre
{
    public class CreateGenreCommand : IRequest<int>
    {
        public string Name { get; set; }
    }

    public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, int>
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public CreateGenreCommandHandler(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateGenreCommand command, CancellationToken cancellationToken)
        {
            var genre = _mapper.Map<Genre>(command);
            genre = await _genreRepository.AddAsync(genre);
            return genre.Id;
        }
    }
}
