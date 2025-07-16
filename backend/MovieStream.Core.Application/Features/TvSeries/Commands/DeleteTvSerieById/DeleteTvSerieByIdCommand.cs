using MediatR;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using System.Net;

namespace MovieStream.Core.Application.Features.TvSeries.Commands.DeleteTvSerieById
{
    public class DeleteTvSerieByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteTvSerieByIdCommandHandler : IRequestHandler<DeleteTvSerieByIdCommand, Response<int>>
    {
        private readonly ITvSerieRepository _tvSerieRepository;

        public DeleteTvSerieByIdCommandHandler(ITvSerieRepository tvSerieRepository)
        {
            _tvSerieRepository = tvSerieRepository;
        }

        public async Task<Response<int>> Handle(DeleteTvSerieByIdCommand command, CancellationToken cancellationToken)
        {
            var tvSerie = await _tvSerieRepository.GetByIdAsync(command.Id);

            if (tvSerie == null) throw new ApiException("Tv Serie not found.", (int)HttpStatusCode.NotFound);

            await _tvSerieRepository.DeleteAsync(tvSerie);

            return new Response<int>(tvSerie.Id);
        }
    }
}
