using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Domain.Entities;
using System.Text.Json.Serialization;

namespace MovieStream.Core.Application.Features.TvSeries.Commands.CreateTvSerie
{
    public class CreateTvSerieCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ProductionCompanyId { get; set; }
        public List<int> GenreIds { get; set; }


        [JsonIgnore]
        public string? ImagePath { get; set; }
        public IFormFile ImageFile { get; set; }
    }

    public class CreateTvSerieCommandHandler : IRequestHandler<CreateTvSerieCommand, int>
    {
        private readonly ITvSerieRepository _tvSerieRepository;
        private readonly IMapper _mapper;

        public CreateTvSerieCommandHandler(ITvSerieRepository tvSerieRepository, IMapper mapper)
        {
            _tvSerieRepository = tvSerieRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateTvSerieCommand command, CancellationToken cancellationToken)
        {
            var tvSerie = _mapper.Map<TvSerie>(command);
            tvSerie = await _tvSerieRepository.AddAsync(tvSerie);
            return tvSerie.Id;
        }
    }
}
