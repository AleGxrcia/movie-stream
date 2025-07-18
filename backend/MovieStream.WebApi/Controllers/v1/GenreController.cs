using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieStream.Core.Application.DTOs.Genre;
using MovieStream.Core.Application.Features.Genres.Commands.CreateGenre;
using MovieStream.Core.Application.Features.Genres.Commands.DeleteGenreById;
using MovieStream.Core.Application.Features.Genres.Commands.UpdateGenre;
using MovieStream.Core.Application.Features.Genres.Queries.GetAllGenres;
using MovieStream.Core.Application.Features.Genres.Queries.GetGenreById;

namespace MovieStream.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class GenreController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenreDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllGenresQuery()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenreDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetGenreByIdQuery() { Id = id }));
        }     
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(CreateGenreCommand command)
        {
            var response = await Mediator.Send(command);

            return CreatedAtAction(nameof(Get), new { id = response.Data });
        }   
        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveGenreDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, UpdateGenreCommand command)
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
            await Mediator.Send(new DeleteGenreByIdCommand { Id = id });

            return NoContent();
        }
    }
}
