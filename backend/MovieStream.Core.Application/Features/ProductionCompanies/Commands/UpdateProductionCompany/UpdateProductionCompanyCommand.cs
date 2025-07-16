using AutoMapper;
using MediatR;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using MovieStream.Core.Domain.Entities;
using System.Net;

namespace MovieStream.Core.Application.Features.ProductionCompanies.Commands.UpdateProductionCompany
{
    public class UpdateProductionCompanyCommand : IRequest<Response<ProductionCompanyUpdateResponse>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateProductionCompanyCommandHandler : IRequestHandler<UpdateProductionCompanyCommand,
        Response<ProductionCompanyUpdateResponse>>
    {
        private readonly IProductionCompanyRepository _prodCompanyRepository;
        private readonly IMapper _mapper;

        public UpdateProductionCompanyCommandHandler(IProductionCompanyRepository prodCompanyRepository, IMapper mapper)
        {
            _prodCompanyRepository = prodCompanyRepository;
            _mapper = mapper;
        }

        public async Task<Response<ProductionCompanyUpdateResponse>> Handle(UpdateProductionCompanyCommand command,
            CancellationToken cancellationToken)
        {
            var prodCompany = await _prodCompanyRepository.GetByIdAsync(command.Id);

            if (prodCompany == null) throw new ApiException("Production Company not found.", (int)HttpStatusCode.NotFound);

            prodCompany = _mapper.Map<ProductionCompany>(command);

            await _prodCompanyRepository.UpdateAsync(prodCompany, prodCompany.Id);

            var prodCompanyResponse = _mapper.Map<ProductionCompanyUpdateResponse>(prodCompany);

            return new Response<ProductionCompanyUpdateResponse>(prodCompanyResponse);
        }
    }
}
