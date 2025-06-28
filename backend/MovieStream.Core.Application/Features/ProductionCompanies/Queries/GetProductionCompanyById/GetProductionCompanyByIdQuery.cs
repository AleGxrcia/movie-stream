using AutoMapper;
using MediatR;
using MovieStream.Core.Application.DTOs.ProductionCompany;
using MovieStream.Core.Application.Interfaces.Repositories;

namespace MovieStream.Core.Application.Features.ProductionCompanies.Queries.GetProductionCompanyById
{
    public class GetProductionCompanyByIdQuery : IRequest<ProductionCompanyDto>
    {
        public int Id { get; set; }
    }

    public class GetProductionCompanyByIdQueryHandler : IRequestHandler<GetProductionCompanyByIdQuery, ProductionCompanyDto>
    {
        private readonly IProductionCompanyRepository _prodCompanyRepository;
        private readonly IMapper _mapper;

        public GetProductionCompanyByIdQueryHandler(IProductionCompanyRepository prodCompanyRepository, IMapper mapper)
        {
            _prodCompanyRepository = prodCompanyRepository;
            _mapper = mapper;
        }

        public async Task<ProductionCompanyDto> Handle(GetProductionCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            var prodCompany = await GetByIdDto(request.Id);
            if (prodCompany == null) throw new Exception("Production Company not found.");
            return prodCompany;
        }

        private async Task<ProductionCompanyDto> GetByIdDto(int id)
        {
            var prodCompanyList = await _prodCompanyRepository.GetByIdAsync(id);
            return _mapper.Map<ProductionCompanyDto>(prodCompanyList);
        }
    }
}
