using AutoMapper;
using MediatR;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Domain.Entities;

namespace MovieStream.Core.Application.Features.ProductionCompanies.Commands.CreateProductionCompany
{
    public class CreateProductionCompanyCommand : IRequest<int>
    {
        public string Name { get; set; }
    }

    public class CreateProductionCompanyCommandHandler : IRequestHandler<CreateProductionCompanyCommand, int>
    {
        private readonly IProductionCompanyRepository _prodCompanyRepository;
        private readonly IMapper _mapper;

        public CreateProductionCompanyCommandHandler(IProductionCompanyRepository prodCompanyRepository, IMapper mapper)
        {
            _prodCompanyRepository = prodCompanyRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateProductionCompanyCommand command, CancellationToken cancellationToken)
        {
            var prodCompany = _mapper.Map<ProductionCompany>(command);
            prodCompany = await _prodCompanyRepository.AddAsync(prodCompany);
            return prodCompany.Id;
        }
    }
}
