using MediatR;
using MovieStream.Core.Application.Interfaces.Repositories;

namespace MovieStream.Core.Application.Features.TvSeries.Commands.DeleteTvSerieById
{
    public class DeleteTvSerieByIdCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class DeleteTvSerieByIdCommandHandler : IRequestHandler<DeleteTvSerieByIdCommand, int>
    {
        private readonly ITvSerieRepository _tvSerieRepository;

        public DeleteTvSerieByIdCommandHandler(ITvSerieRepository tvSerieRepository)
        {
            _tvSerieRepository = tvSerieRepository;
        }

        public async Task<int> Handle(DeleteTvSerieByIdCommand command, CancellationToken cancellationToken)
        {
            var tvSerie = await _tvSerieRepository.GetByIdAsync(command.Id);

            if (tvSerie == null) throw new Exception("Tv Serie not found.");

            await _tvSerieRepository.DeleteAsync(tvSerie);

            return tvSerie.Id;
        }
    }
}
