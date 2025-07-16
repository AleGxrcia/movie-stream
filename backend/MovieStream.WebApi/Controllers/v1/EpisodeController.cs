using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieStream.Core.Application.Common.Parameters.Episodes;
using MovieStream.Core.Application.DTOs.Episode;
using MovieStream.Core.Application.Features.Episodes.Commands.CreateEpisode;
using MovieStream.Core.Application.Features.Episodes.Commands.DeleteEpisodeById;
using MovieStream.Core.Application.Features.Episodes.Commands.UpdateEpisode;
using MovieStream.Core.Application.Features.Episodes.Queries.GetAllEpisodes;
using MovieStream.Core.Application.Features.Episodes.Queries.GetEpisodeDetailsById;

namespace MovieStream.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class EpisodeController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EpisodeDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] EpisodeParameters parameters)
        {
            return Ok(await Mediator.Send(new GetAllEpisodesQuery() { Parameters = parameters }));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EpisodeDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetEpisodeDetailsByIdQuery() { Id = id }));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(CreateEpisodeCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveEpisodeDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, UpdateEpisodeCommand command)
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
            await Mediator.Send(new DeleteEpisodeByIdCommand { Id = id });
            return NoContent();
        }
    }
}
