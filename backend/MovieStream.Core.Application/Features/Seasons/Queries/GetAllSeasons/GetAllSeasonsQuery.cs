using AutoMapper;
using MediatR;
using MovieStream.Core.Application.Common.Parameters.Seasons;
using MovieStream.Core.Application.DTOs.Season;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using System.Net;

namespace MovieStream.Core.Application.Features.Seasons.Queries.GetAllSeasons
{
    public class GetAllSeasonsQuery : IRequest<Response<PagedList<SeasonDto>>>
    {
        public SeasonParameters Parameters { get; set; }
    }

    public class GetAllSeasonsQueryHandler : IRequestHandler<GetAllSeasonsQuery, Response<PagedList<SeasonDto>>>
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IMapper _mapper;

        public GetAllSeasonsQueryHandler(ISeasonRepository seasonRepository, IMapper mapper)
        {
            _seasonRepository = seasonRepository;
            _mapper = mapper;
        }

        public async Task<Response<PagedList<SeasonDto>>> Handle(GetAllSeasonsQuery request, CancellationToken cancellationToken)
        {
            var filters = request.Parameters;
            var pagedSeasonsDto = await GetAllDtoWithFilters(filters);

            if (pagedSeasonsDto == null || pagedSeasonsDto.Count == 0)
            {
                throw new ApiException("Seasons not found.", (int)HttpStatusCode.NotFound);
            }

            return new Response<PagedList<SeasonDto>>(pagedSeasonsDto);
        }

        private async Task<PagedList<SeasonDto>> GetAllDtoWithFilters(SeasonParameters parameters)
        {
            var seasonList = await _seasonRepository.GetAllWithFilters(parameters);
            var seasonDtoList = _mapper.Map<List<SeasonDto>>(seasonList);

            return new PagedList<SeasonDto>(
                seasonDtoList,
                seasonList.MetaData.TotalCount,
                parameters.PageNumber,
                parameters.PageSize
             );
        }
    }
}