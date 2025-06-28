using AutoMapper;
using MediatR;
using MovieStream.Core.Application.DTOs.ProductionCompany;
using MovieStream.Core.Application.Interfaces.Repositories;

namespace MovieStream.Core.Application.Features.ProductionCompanies.Queries.GetAllProductionCompanies
{
    public class GetAllProductionCompaniesQuery : IRequest<IList<ProductionCompanyDto>>
    {
    }

    public class GetAllProductionCompaniesQueryHandler : IRequestHandler<GetAllProductionCompaniesQuery, IList<ProductionCompanyDto>>
    {
        private readonly IProductionCompanyRepository _prodCompanyRepository;
        private readonly IMapper _mapper;

        public GetAllProductionCompaniesQueryHandler(IProductionCompanyRepository prodCompanyRepository, IMapper mapper)
        {
            _prodCompanyRepository = prodCompanyRepository;
            _mapper = mapper;
        }

        public async Task<IList<ProductionCompanyDto>> Handle(GetAllProductionCompaniesQuery request, CancellationToken cancellationToken)
        {
            var prodCompanyList = await GetAllDtoWithFilters();
            if (prodCompanyList == null || prodCompanyList.Count == 0) throw new Exception("Production Companies not found.");
            return prodCompanyList;
        }

        private async Task<List<ProductionCompanyDto>> GetAllDtoWithFilters()
        {
            var prodCompanyList = await _prodCompanyRepository.GetAllAsync();
            return _mapper.Map<List<ProductionCompanyDto>>(prodCompanyList);
        }
    }
}
