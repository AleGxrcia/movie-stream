using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieStream.Core.Application.Common.Parameters.Movies;
using MovieStream.Core.Application.DTOs.Movie;
using MovieStream.Core.Application.Features.Movies.Commands.CreateMovie;
using MovieStream.Core.Application.Features.Movies.Commands.DeleteMovieById;
using MovieStream.Core.Application.Features.Movies.Commands.UpdateMovie;
using MovieStream.Core.Application.Features.Movies.Queries.GetAllMovies;
using MovieStream.Core.Application.Features.Movies.Queries.GetMovieById;
using MovieStream.Core.Application.Interfaces.Services;

namespace MovieStream.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    //[Authorize(Roles = nameof(Roles.Admin))]
    public class MovieController : BaseApiController
    {
        private readonly IFileManagerService _fileManagerService;
        private readonly IMapper _mapper;

        public MovieController(IFileManagerService fileManagerService, IMapper mapper)
        {
            _fileManagerService = fileManagerService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MovieDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] MovieParameters parameters)
        {
            return Ok(await Mediator.Send(new GetAllMoviesQuery() { Parameters = parameters }));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MovieDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetMovieByIdQuery() { Id = id }));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromForm] CreateMovieCommand command)
        {
            var response = await Mediator.Send(command);

            if (command.ImageFile != null)
            {
                var imagePath = await _fileManagerService.UploadFileAsync(command.ImageFile, response.Data, "Movies");

                var updateMovie = _mapper.Map<UpdateMovieCommand>(command);
                updateMovie.Id = response.Data;
                updateMovie.ImagePath = imagePath;

                await Mediator.Send(updateMovie);
            }

            return CreatedAtAction(nameof(Get), new { id = response.Data }, response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MovieUpdateResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, UpdateMovieCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != command.Id)
            {
                return BadRequest();
            }

            if (command.ImageFile != null)
            {
                var response = await Mediator.Send(new GetMovieByIdQuery() { Id = id });

                var imagePath = await _fileManagerService.UploadFileAsync(command.ImageFile, id, "Movies", response.Data.ImagePath);
                command.ImagePath = imagePath;

                await Mediator.Send(command);
            }

            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteMovieByIdCommand { Id = id });
            return NoContent();
        }
    }
}
