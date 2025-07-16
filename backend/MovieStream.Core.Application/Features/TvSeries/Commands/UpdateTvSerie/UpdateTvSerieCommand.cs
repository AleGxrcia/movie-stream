using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using MovieStream.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace MovieStream.Core.Application.Features.TvSeries.Commands.UpdateTvSerie
{
    public class UpdateTvSerieCommand : IRequest<Response<TvSerieUpdateResponse>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ProductionCompanyId { get; set; }
        public List<int> GenreIds { get; set; }


        [SwaggerIgnore]
        public string? ImagePath { get; set; }
        public IFormFile ImageFile { get; set; }
    }

    public class UpdateTvSerieCommandHandler : IRequestHandler<UpdateTvSerieCommand,
        Response<TvSerieUpdateResponse>>
    {
        private readonly ITvSerieRepository _tvSerieRepository;
        private readonly IMapper _mapper;

        public UpdateTvSerieCommandHandler(ITvSerieRepository tvSerieRepository, IMapper mapper)
        {
            _tvSerieRepository = tvSerieRepository;
            _mapper = mapper;
        }

        public async Task<Response<TvSerieUpdateResponse>> Handle(UpdateTvSerieCommand command,
            CancellationToken cancellationToken)
        {
            var tvSerie = await _tvSerieRepository.GetByIdAsync(command.Id);

            if (tvSerie == null) throw new ApiException("Tv Serie not found.", (int)HttpStatusCode.NotFound);

            tvSerie = _mapper.Map<TvSerie>(command);

            await _tvSerieRepository.UpdateAsync(tvSerie, tvSerie.Id);

            var tvSerieResponse = _mapper.Map<TvSerieUpdateResponse>(tvSerie);

            return new Response<TvSerieUpdateResponse>(tvSerieResponse);
        }
    }
}
