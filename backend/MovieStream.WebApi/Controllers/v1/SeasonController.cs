using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieStream.Core.Application.Common.Parameters.Seasons;
using MovieStream.Core.Application.DTOs.Season;
using MovieStream.Core.Application.Features.Seasons.Commands.CreateSeason;
using MovieStream.Core.Application.Features.Seasons.Commands.DeleteSeasonById;
using MovieStream.Core.Application.Features.Seasons.Commands.UpdateSeason;
using MovieStream.Core.Application.Features.Seasons.Queries.GetAllSeasons;
using MovieStream.Core.Application.Features.Seasons.Queries.GetSeasonDetailsById;

namespace MovieStream.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class SeasonController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SeasonDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] SeasonParameters parameters)
        {
            return Ok(await Mediator.Send(new GetAllSeasonsQuery() { Parameters = parameters }));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SeasonDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetSeasonDetailsByIdQuery() { Id = id }));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(CreateSeasonCommand command)
        {
            var response = await Mediator.Send(command);

            return CreatedAtAction(nameof(Get), new { id = response.Data });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveSeasonDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, UpdateSeasonCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
            await Mediator.Send(new DeleteSeasonByIdCommand { Id = id });
            return NoContent();
        }
    }
}
