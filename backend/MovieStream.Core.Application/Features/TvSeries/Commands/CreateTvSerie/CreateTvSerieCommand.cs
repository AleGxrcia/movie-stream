using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using MovieStream.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieStream.Core.Application.Features.TvSeries.Commands.CreateTvSerie
{
    public class CreateTvSerieCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ProductionCompanyId { get; set; }
        public List<int> GenreIds { get; set; }


        [SwaggerIgnore]
        public string? ImageUrl { get; set; }
        public IFormFile ImageFile { get; set; }
    }

    public class CreateTvSerieCommandHandler : IRequestHandler<CreateTvSerieCommand, Response<int>>
    {
        private readonly ITvSerieRepository _tvSerieRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public CreateTvSerieCommandHandler(ITvSerieRepository tvSerieRepository, IGenreRepository genreRepository,
            IMapper mapper)
        {
            _tvSerieRepository = tvSerieRepository;
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateTvSerieCommand command, CancellationToken cancellationToken)
        {
            var tvSerie = _mapper.Map<TvSerie>(command);
            var genres = await _genreRepository.GetByIdsAsync(command.GenreIds);

            tvSerie.Genres = genres;

            tvSerie = await _tvSerieRepository.AddAsync(tvSerie);
            return new Response<int>(tvSerie.Id);
        }
    }
}
