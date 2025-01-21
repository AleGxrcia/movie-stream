using MediatR;
using MovieStream.Core.Application.Interfaces.Repositories;

namespace MovieStream.Core.Application.Features.Seasons.Commands.DeleteSeasonById
{
    public class DeleteSeasonByIdCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class DeleteSeasonByIdCommandHandler : IRequestHandler<DeleteSeasonByIdCommand, int>
    {
        private readonly ISeasonRepository _seasonRepository;

        public DeleteSeasonByIdCommandHandler(ISeasonRepository seasonRepository)
        {
            _seasonRepository = seasonRepository;
        }

        public async Task<int> Handle(DeleteSeasonByIdCommand command, CancellationToken cancellationToken)
        {
            var season = await _seasonRepository.GetByIdAsync(command.Id);

            if (season == null) throw new Exception("Season not found.");

            await _seasonRepository.DeleteAsync(season);

            return season.Id;
        }
    }
}
