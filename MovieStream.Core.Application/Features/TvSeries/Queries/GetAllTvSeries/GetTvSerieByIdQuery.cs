using AutoMapper;
using MediatR;
using MovieStream.Core.Application.DTOs.TvSerie;
using MovieStream.Core.Application.Interfaces.Repositories;

namespace MovieStream.Core.Application.Features.TvSeries.Queries.GetAllTvSeries
{
    public class GetTvSerieByIdQuery : IRequest<TvSerieDto>
    {
        public int Id { get; set; }
    }

    public class GetTvSerieByIdQueryHandler : IRequestHandler<GetTvSerieByIdQuery, TvSerieDto>
    {
        private readonly ITvSerieRepository _tvSerieRepository;
        private readonly IMapper _mapper;

        public GetTvSerieByIdQueryHandler(ITvSerieRepository tvSerieRepository, IMapper mapper)
        {
            _tvSerieRepository = tvSerieRepository;
            _mapper = mapper;
        }

        public async Task<TvSerieDto> Handle(GetTvSerieByIdQuery request, CancellationToken cancellationToken)
        {
            var tvSerie = await GetByIdDto(request.Id);
            if (tvSerie == null) throw new Exception("TvSerie not found.");
            return tvSerie;
        }

        private async Task<TvSerieDto> GetByIdDto(int id)
        {
            var tvSerie = await _tvSerieRepository.GetByIdWithInclude(id);
            return _mapper.Map<TvSerieDto>(tvSerie);
        }
    }
}
