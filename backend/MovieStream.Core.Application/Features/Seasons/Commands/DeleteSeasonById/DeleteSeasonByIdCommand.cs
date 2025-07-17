using MediatR;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using System.Net;

namespace MovieStream.Core.Application.Features.Seasons.Commands.DeleteSeasonById
{
    public class DeleteSeasonByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteSeasonByIdCommandHandler : IRequestHandler<DeleteSeasonByIdCommand, Response<int>>
    {
        private readonly ISeasonRepository _seasonRepository;

        public DeleteSeasonByIdCommandHandler(ISeasonRepository seasonRepository)
        {
            _seasonRepository = seasonRepository;
        }

        public async Task<Response<int>> Handle(DeleteSeasonByIdCommand command, CancellationToken cancellationToken)
        {
            var season = await _seasonRepository.GetByIdAsync(command.Id);

            if (season == null) throw new ApiException("Season not found.", (int)HttpStatusCode.NotFound);

            await _seasonRepository.DeleteAsync(season);

            return new Response<int>(season.Id);
        }
    }
}
