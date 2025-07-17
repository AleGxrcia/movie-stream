using AutoMapper;
using MediatR;
using MovieStream.Core.Application.DTOs.TvSerie;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using System.Net;

namespace MovieStream.Core.Application.Features.TvSeries.Queries.GetTvSerieById
{
    public class GetTvSerieByIdQuery : IRequest<Response<TvSerieDto>>
    {
        public int Id { get; set; }
    }

    public class GetTvSerieByIdQueryHandler : IRequestHandler<GetTvSerieByIdQuery, Response<TvSerieDto>>
    {
        private readonly ITvSerieRepository _tvSerieRepository;
        private readonly IMapper _mapper;

        public GetTvSerieByIdQueryHandler(ITvSerieRepository tvSerieRepository, IMapper mapper)
        {
            _tvSerieRepository = tvSerieRepository;
            _mapper = mapper;
        }

        public async Task<Response<TvSerieDto>> Handle(GetTvSerieByIdQuery request, CancellationToken cancellationToken)
        {
            var tvSerie = await GetByIdDto(request.Id);
            if (tvSerie == null) throw new ApiException("TvSerie not found.", (int)HttpStatusCode.NotFound);
            return new Response<TvSerieDto>(tvSerie);
        }

        private async Task<TvSerieDto> GetByIdDto(int id)
        {
            var tvSerie = await _tvSerieRepository.GetByIdWithInclude(id);
            return _mapper.Map<TvSerieDto>(tvSerie);
        }
    }
}
