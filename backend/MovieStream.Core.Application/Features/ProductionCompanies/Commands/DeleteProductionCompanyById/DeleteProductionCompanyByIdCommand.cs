using MediatR;
using MovieStream.Core.Application.Interfaces.Repositories;

namespace MovieStream.Core.Application.Features.ProductionCompanies.Commands.DeleteProductionCompanyById
{
    public class DeleteProductionCompanyByIdCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class DeleteProductionCompanyByIdCommandHandler : IRequestHandler<DeleteProductionCompanyByIdCommand, int>
    {
        private readonly IProductionCompanyRepository _prodCompanyRepository;

        public DeleteProductionCompanyByIdCommandHandler(IProductionCompanyRepository prodCompanyRepository)
        {
            _prodCompanyRepository = prodCompanyRepository;
        }

        public async Task<int> Handle(DeleteProductionCompanyByIdCommand command, CancellationToken cancellationToken)
        {
            var prodCompany = await _prodCompanyRepository.GetByIdAsync(command.Id);

            if (prodCompany == null) throw new Exception("Production Company not found.");

            await _prodCompanyRepository.DeleteAsync(prodCompany);

            return prodCompany.Id;
        }
    }
}
