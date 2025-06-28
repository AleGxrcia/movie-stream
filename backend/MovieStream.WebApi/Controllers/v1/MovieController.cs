using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Get([FromQuery] GetAllMoviesParameters filters)
        {
            try
            {
                return Ok(await Mediator.Send(new GetAllMoviesQuery()
                {
                    FilterBy = filters.FilterBy,
                    FilterValue = filters.FilterValue,
                    PageNumber = filters.PageNumber,
                    PageSize = filters.PageSize,
                    SortColumn = filters.SortColumn,
                    SortOrder = filters.SortOrder
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MovieDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await Mediator.Send(new GetMovieByIdQuery() { Id = id }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromForm] CreateMovieCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var id = await Mediator.Send(command);

                if (command.ImageFile != null)
                {
                    var imagePath = await _fileManagerService.UploadFileAsync(command.ImageFile, id, "Movies");

                    var updateMovie = _mapper.Map<UpdateMovieCommand>(command);
                    updateMovie.Id = id;
                    updateMovie.ImagePath = imagePath;

                    await Mediator.Send(updateMovie);
                }

                var movie = await Mediator.Send(new GetMovieByIdQuery() { Id = id });

                return Ok(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MovieUpdateResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, UpdateMovieCommand command)
        {
            try
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
                    var movie = await Mediator.Send(new GetMovieByIdQuery() { Id = id });

                    var imagePath = await _fileManagerService.UploadFileAsync(command.ImageFile, id, "Movies", movie.ImagePath);
                    command.ImagePath = imagePath;

                    await Mediator.Send(command);
                }

                return Ok(await Mediator.Send(command));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await Mediator.Send(new DeleteMovieByIdCommand { Id = id });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
