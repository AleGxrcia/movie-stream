using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieStream.Core.Application.DTOs.ProductionCompany;
using MovieStream.Core.Application.Features.ProductionCompanies.Commands.CreateProductionCompany;
using MovieStream.Core.Application.Features.ProductionCompanies.Commands.DeleteProductionCompanyById;
using MovieStream.Core.Application.Features.ProductionCompanies.Commands.UpdateProductionCompany;
using MovieStream.Core.Application.Features.ProductionCompanies.Queries.GetAllProductionCompanies;
using MovieStream.Core.Application.Features.ProductionCompanies.Queries.GetProductionCompanyById;

namespace MovieStream.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ProductionCompanyController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductionCompanyDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllProductionCompaniesQuery()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductionCompanyDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetProductionCompanyByIdQuery() { Id = id }));
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(CreateProductionCompanyCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(await Mediator.Send(command));
        }   
        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveProductionCompanyDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, UpdateProductionCompanyCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (id != command.Id)
            {
                return BadRequest();
            }

            return Ok(await Mediator.Send(command));
        }  
        
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteProductionCompanyByIdCommand { Id = id });
            return NoContent();
        }
    }
}
