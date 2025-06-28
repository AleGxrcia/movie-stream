using AutoMapper;
using MediatR;
using MovieStream.Core.Application.DTOs.Season;
using MovieStream.Core.Application.Interfaces.Repositories;

namespace MovieStream.Core.Application.Features.Seasons.Queries.GetSeasonDetailsById
{
    public class GetSeasonDetailsByIdQuery : IRequest<SeasonDto>
    {
        public int Id { get; set; }
    }

    public class GetSeasonDetailsByIdQueryHandler : IRequestHandler<GetSeasonDetailsByIdQuery, SeasonDto>
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IMapper _mapper;

        public GetSeasonDetailsByIdQueryHandler(ISeasonRepository seasonRepository, IMapper mapper)
        {
            _seasonRepository = seasonRepository;
            _mapper = mapper;
        }

        public async Task<SeasonDto> Handle(GetSeasonDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var season = await GetByIdDto(request.Id);
            if (season == null) throw new Exception("Season not found.");
            return season;
        }

        private async Task<SeasonDto> GetByIdDto(int id)
        {
            var season = await _seasonRepository.GetByIdAsync(id);
            return _mapper.Map<SeasonDto>(season);
        }
    }
}
