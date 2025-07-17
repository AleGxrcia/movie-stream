using MediatR;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using System.Net;

namespace MovieStream.Core.Application.Features.ProductionCompanies.Commands.DeleteProductionCompanyById
{
    public class DeleteProductionCompanyByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteProductionCompanyByIdCommandHandler : IRequestHandler<DeleteProductionCompanyByIdCommand, Response<int>>
    {
        private readonly IProductionCompanyRepository _prodCompanyRepository;

        public DeleteProductionCompanyByIdCommandHandler(IProductionCompanyRepository prodCompanyRepository)
        {
            _prodCompanyRepository = prodCompanyRepository;
        }

        public async Task<Response<int>> Handle(DeleteProductionCompanyByIdCommand command, CancellationToken cancellationToken)
        {
            var prodCompany = await _prodCompanyRepository.GetByIdAsync(command.Id);

            if (prodCompany == null) throw new ApiException("Production Company not found.", (int)HttpStatusCode.NotFound);

            await _prodCompanyRepository.DeleteAsync(prodCompany);

            return new Response<int>(prodCompany.Id);
        }
    }
}
