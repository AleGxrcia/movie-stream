using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieStream.Core.Application.Common.Parameters.TvSeries;
using MovieStream.Core.Application.DTOs.TvSerie;
using MovieStream.Core.Application.Features.TvSeries.Commands.CreateTvSerie;
using MovieStream.Core.Application.Features.TvSeries.Commands.DeleteTvSerieById;
using MovieStream.Core.Application.Features.TvSeries.Commands.UpdateTvSerie;
using MovieStream.Core.Application.Features.TvSeries.Queries.GetAllTvSeries;
using MovieStream.Core.Application.Features.TvSeries.Queries.GetTvSerieById;
using MovieStream.Core.Application.Interfaces.Services;

namespace MovieStream.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    //[Authorize(Roles = nameof(Roles.Admin))]
    public class TvSerieController : BaseApiController
    {
        private readonly IFileManagerService _fileManagerService;
        private readonly IMapper _mapper;

        public TvSerieController(IFileManagerService fileManagerService, IMapper mapper)
        {
            _fileManagerService = fileManagerService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TvSerieDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] TvSerieParameters parameters)
        {
            return Ok(await Mediator.Send(new GetAllTvSeriesQuery() { Parameters = parameters }));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TvSerieDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetTvSerieByIdQuery() { Id = id }));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromForm] CreateTvSerieCommand command)
        {
            var response = await Mediator.Send(command);

            if (command.ImageFile != null)
            {
                var imagePath = await _fileManagerService.UploadFileAsync(command.ImageFile, response.Data, "TvSeries");

                var updateTvSerie = _mapper.Map<UpdateTvSerieCommand>(command);
                updateTvSerie.Id = response.Data;
                updateTvSerie.ImagePath = imagePath;

                await Mediator.Send(updateTvSerie);
            }

            return CreatedAtAction(nameof(Get), new { id = response.Data }, response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TvSerieUpdateResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, UpdateTvSerieCommand command)
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
                var response = await Mediator.Send(new GetTvSerieByIdQuery() { Id = id });

                var imagePath = await _fileManagerService.UploadFileAsync(command.ImageFile, id, "TvSeries", response.Data.ImagePath);
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
            await Mediator.Send(new DeleteTvSerieByIdCommand { Id = id });
            return NoContent();
        }
    }
}
