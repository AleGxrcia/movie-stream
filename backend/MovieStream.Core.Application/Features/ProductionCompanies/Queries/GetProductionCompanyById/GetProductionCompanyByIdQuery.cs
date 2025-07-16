using AutoMapper;
using MediatR;
using MovieStream.Core.Application.DTOs.ProductionCompany;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using System.Net;

namespace MovieStream.Core.Application.Features.ProductionCompanies.Queries.GetProductionCompanyById
{
    public class GetProductionCompanyByIdQuery : IRequest<Response<ProductionCompanyDto>>
    {
        public int Id { get; set; }
    }

    public class GetProductionCompanyByIdQueryHandler : IRequestHandler<GetProductionCompanyByIdQuery, Response<ProductionCompanyDto>>
    {
        private readonly IProductionCompanyRepository _prodCompanyRepository;
        private readonly IMapper _mapper;

        public GetProductionCompanyByIdQueryHandler(IProductionCompanyRepository prodCompanyRepository, IMapper mapper)
        {
            _prodCompanyRepository = prodCompanyRepository;
            _mapper = mapper;
        }

        public async Task<Response<ProductionCompanyDto>> Handle(GetProductionCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            var prodCompany = await GetByIdDto(request.Id);
            if (prodCompany == null) throw new ApiException("Production Company not found.", (int)HttpStatusCode.NotFound);
            return new Response<ProductionCompanyDto>(prodCompany);
        }

        private async Task<ProductionCompanyDto> GetByIdDto(int id)
        {
            var prodCompany = await _prodCompanyRepository.GetByIdAsync(id);
            return _mapper.Map<ProductionCompanyDto>(prodCompany);
        }
    }
}
