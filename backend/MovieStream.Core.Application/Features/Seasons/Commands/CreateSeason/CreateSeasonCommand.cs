using AutoMapper;
using MediatR;
using MovieStream.Core.Application.DTOs.Season;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.Features.Seasons.Commands.CreateSeason
{
    public class CreateSeasonCommand : IRequest<Response<int>>
    {
        public int SeasonNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int TvSerieId { get; set; }
    }

    public class CreateSeasonCommandHandler : IRequestHandler<CreateSeasonCommand, Response<int>>
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IMapper _mapper;

        public CreateSeasonCommandHandler(ISeasonRepository seasonRepository, IMapper mapper)
        {
            _seasonRepository = seasonRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateSeasonCommand command, CancellationToken cancellationToken)
        {
            var season = _mapper.Map<Season>(command);
            season = await _seasonRepository.AddAsync(season);
            return new Response<int>(season.Id);
        }
    }
}
