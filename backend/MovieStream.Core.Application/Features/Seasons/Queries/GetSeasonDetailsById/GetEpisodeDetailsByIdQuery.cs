using AutoMapper;
using MediatR;
using MovieStream.Core.Application.DTOs.Season;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using System.Net;

namespace MovieStream.Core.Application.Features.Seasons.Queries.GetSeasonDetailsById
{
    public class GetSeasonDetailsByIdQuery : IRequest<Response<SeasonDto>>
    {
        public int Id { get; set; }
    }

    public class GetSeasonDetailsByIdQueryHandler : IRequestHandler<GetSeasonDetailsByIdQuery, Response<SeasonDto>>
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IMapper _mapper;

        public GetSeasonDetailsByIdQueryHandler(ISeasonRepository seasonRepository, IMapper mapper)
        {
            _seasonRepository = seasonRepository;
            _mapper = mapper;
        }

        public async Task<Response<SeasonDto>> Handle(GetSeasonDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var season = await GetByIdDto(request.Id);
            if (season == null) throw new ApiException("Season not found.", (int)HttpStatusCode.NotFound);
            return new Response<SeasonDto>(season);
        }

        private async Task<SeasonDto> GetByIdDto(int id)
        {
            var season = await _seasonRepository.GetByIdAsync(id);
            return _mapper.Map<SeasonDto>(season);
        }
    }
}
