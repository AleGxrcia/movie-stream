using AutoMapper;
using MediatR;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.Features.Seasons.Commands.CreateSeason
{
    public class CreateSeasonCommand : IRequest<int>
    {
        public int SeasonNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int TvSerieId { get; set; }
    }

    public class CreateSeasonCommandHandler : IRequestHandler<CreateSeasonCommand, int>
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IMapper _mapper;

        public CreateSeasonCommandHandler(ISeasonRepository seasonRepository, IMapper mapper)
        {
            _seasonRepository = seasonRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateSeasonCommand command, CancellationToken cancellationToken)
        {
            var season = _mapper.Map<Season>(command);
            season = await _seasonRepository.AddAsync(season);
            return season.Id;
        }
    }
}
