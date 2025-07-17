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
    public class GetAllProductionCompaniesQuery : IRequest<Response<PagedList<ProductionCompanyDto>>>
    {
        public RequestParameters Parameters { get; set; }
    }

    public class GetAllProductionCompaniesQueryHandler : IRequestHandler<GetAllProductionCompaniesQuery,
        Response<PagedList<ProductionCompanyDto>>>
    {
        private readonly IProductionCompanyRepository _prodCompanyRepository;
        private readonly IMapper _mapper;

        public GetAllProductionCompaniesQueryHandler(IProductionCompanyRepository prodCompanyRepository, IMapper mapper)
        {
            _prodCompanyRepository = prodCompanyRepository;
            _mapper = mapper;
        }

        public async Task<Response<PagedList<ProductionCompanyDto>>> Handle(GetAllProductionCompaniesQuery request,
            CancellationToken cancellationToken)
        {
            var filters = request.Parameters;
            var pagedProdCompaniesDto = await GetAllDtoWithFilters(filters);

            if (pagedProdCompaniesDto == null || pagedProdCompaniesDto.Count == 0) 
            {
                throw new ApiException("Production Companies not found.", (int)HttpStatusCode.NotFound);
            } 

            return new Response<PagedList<ProductionCompanyDto>>(pagedProdCompaniesDto, pagedProdCompaniesDto.MetaData);
        }

        private async Task<PagedList<ProductionCompanyDto>> GetAllDtoWithFilters(RequestParameters parameters)
        {
            var prodCompanyList = await _prodCompanyRepository.GetAllWithFilters(parameters);
            var prodCompanyDtoList = _mapper.Map<List<ProductionCompanyDto>>(prodCompanyList);

            return new PagedList<ProductionCompanyDto>(
                prodCompanyDtoList,
                prodCompanyList.MetaData.TotalCount,
                parameters.PageNumber,
                parameters.PageSize
             );
        }
    }
}
