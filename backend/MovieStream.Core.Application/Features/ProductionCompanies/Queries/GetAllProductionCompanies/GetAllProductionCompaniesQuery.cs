using AutoMapper;
using MediatR;
using MovieStream.Core.Application.Common.Parameters.Base;
using MovieStream.Core.Application.DTOs.ProductionCompany;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using System.Net;

namespace MovieStream.Core.Application.Features.ProductionCompanies.Queries.GetAllProductionCompanies
{
    public class GetAllProductionCompaniesQuery : IRequest<Response<List<ProductionCompanyDto>>>
    {
        public RequestParameters Parameters { get; set; }
    }

    public class GetAllProductionCompaniesQueryHandler : IRequestHandler<GetAllProductionCompaniesQuery,
        Response<List<ProductionCompanyDto>>>
    {
        private readonly IProductionCompanyRepository _prodCompanyRepository;
        private readonly IMapper _mapper;

        public GetAllProductionCompaniesQueryHandler(IProductionCompanyRepository prodCompanyRepository, IMapper mapper)
        {
            _prodCompanyRepository = prodCompanyRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<ProductionCompanyDto>>> Handle(GetAllProductionCompaniesQuery request,
            CancellationToken cancellationToken)
        {
            var filters = request.Parameters;
            var pagedProdCompaniesDto = await GetAllDtoWithFilters(filters);

            if (pagedProdCompaniesDto == null || pagedProdCompaniesDto.Count == 0) 
            {
                throw new ApiException("Production Companies not found.", (int)HttpStatusCode.NotFound);
            } 

            return new Response<List<ProductionCompanyDto>>(pagedProdCompaniesDto);
        }

        private async Task<List<ProductionCompanyDto>> GetAllDtoWithFilters(RequestParameters parameters)
        {
            var prodCompanyList = await _prodCompanyRepository.GetAllAsync();
            var prodCompanyDtoList = _mapper.Map<List<ProductionCompanyDto>>(prodCompanyList);

            return prodCompanyDtoList;
        }
    }
}
