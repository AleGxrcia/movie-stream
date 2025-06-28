using AutoMapper;
using MediatR;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.Features.Seasons.Commands.UpdateSeason
{
    public class UpdateSeasonCommand : IRequest<SeasonUpdateResponse>
    {
        public int Id { get; set; }
        public int SeasonNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int TvSerieId { get; set; }
    }

    public class UpdateSeasonCommandHandler : IRequestHandler<UpdateSeasonCommand, SeasonUpdateResponse>
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IMapper _mapper;

        public UpdateSeasonCommandHandler(ISeasonRepository seasonRepository, IMapper mapper)
        {
            _seasonRepository = seasonRepository;
            _mapper = mapper;
        }

        public async Task<SeasonUpdateResponse> Handle(UpdateSeasonCommand command, CancellationToken cancellationToken)
        {
            var season = await _seasonRepository.GetByIdAsync(command.Id);

            if (season == null) throw new Exception("Season not found.");

            season = _mapper.Map<Season>(command);

            await _seasonRepository.UpdateAsync(season, season.Id);

            var seasonResponse = _mapper.Map<SeasonUpdateResponse>(season);

            return seasonResponse;
        }
    }
}
