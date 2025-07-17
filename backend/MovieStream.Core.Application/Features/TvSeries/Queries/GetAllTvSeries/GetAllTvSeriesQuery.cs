using AutoMapper;
using MediatR;
using MovieStream.Core.Application.Common.Parameters.TvSeries;
using MovieStream.Core.Application.DTOs.TvSerie;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using System.Net;

namespace MovieStream.Core.Application.Features.TvSeries.Queries.GetAllTvSeries
{
    public class GetAllTvSeriesQuery : IRequest<Response<PagedList<TvSerieDto>>>
    {
        public TvSerieParameters Parameters { get; set; }
    }

    public class GetAllTvSeriesQueryHandler : IRequestHandler<GetAllTvSeriesQuery, Response<PagedList<TvSerieDto>>>
    {
        private readonly ITvSerieRepository _tvSerieRepository;
        private readonly IMapper _mapper;

        public GetAllTvSeriesQueryHandler(ITvSerieRepository tvSerieRepository, IMapper mapper)
        {
            _tvSerieRepository = tvSerieRepository;
            _mapper = mapper;
        }

        public async Task<Response<PagedList<TvSerieDto>>> Handle(GetAllTvSeriesQuery request, CancellationToken cancellationToken)
        {
            var filters = request.Parameters;
            var pagedTvSeriesDto = await GetAllDtoWithFilters(filters);

            if (pagedTvSeriesDto == null || pagedTvSeriesDto.Count == 0)
            {
                throw new ApiException("TvSeries not found.", (int)HttpStatusCode.NotFound);
            }

            return new Response<PagedList<TvSerieDto>>(pagedTvSeriesDto, pagedTvSeriesDto.MetaData);
        }

        private async Task<PagedList<TvSerieDto>> GetAllDtoWithFilters(TvSerieParameters parameters)
        {
            var tvSerieList = await _tvSerieRepository.GetAllWithFilters(parameters);
            var tvSerieDtoList = _mapper.Map<List<TvSerieDto>>(tvSerieList);

            return new PagedList<TvSerieDto>(
                tvSerieDtoList,
                tvSerieList.MetaData.TotalCount,
                parameters.PageSize,
                parameters.PageSize
            );
        }
    }
}
