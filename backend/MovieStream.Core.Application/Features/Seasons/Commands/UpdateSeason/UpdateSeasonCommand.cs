using AutoMapper;
using MediatR;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using MovieStream.Core.Domain.Entities;
using System.Net;

namespace MovieStream.Core.Application.Features.Seasons.Commands.UpdateSeason
{
    public class UpdateSeasonCommand : IRequest<Response<SeasonUpdateResponse>>
    {
        public int Id { get; set; }
        public int SeasonNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int TvSerieId { get; set; }
    }

    public class UpdateSeasonCommandHandler : IRequestHandler<UpdateSeasonCommand, Response<SeasonUpdateResponse>>
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IMapper _mapper;

        public UpdateSeasonCommandHandler(ISeasonRepository seasonRepository, IMapper mapper)
        {
            _seasonRepository = seasonRepository;
            _mapper = mapper;
        }

        public async Task<Response<SeasonUpdateResponse>> Handle(UpdateSeasonCommand command, CancellationToken cancellationToken)
        {
            var season = await _seasonRepository.GetByIdAsync(command.Id);

            if (season == null) throw new ApiException("Season not found.", (int)HttpStatusCode.NotFound);

            season = _mapper.Map<Season>(command);

            await _seasonRepository.UpdateAsync(season, season.Id);

            var seasonResponse = _mapper.Map<SeasonUpdateResponse>(season);

            return new Response<SeasonUpdateResponse>(seasonResponse);
        }
    }
}
