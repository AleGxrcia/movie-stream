using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStream.Core.Application.DTOs.TvSerie;
using MovieStream.Core.Application.Enums;
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
        public async Task<IActionResult> Get([FromQuery] GetAllTvSeriesParameters filters)
        {
            try
            {
                return Ok(await Mediator.Send(new GetAllTvSeriesQuery()
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TvSerieDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await Mediator.Send(new GetTvSerieByIdQuery() { Id = id }));
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
        public async Task<IActionResult> Post([FromForm] CreateTvSerieCommand command)
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
                    var imagePath = await _fileManagerService.UploadFileAsync(command.ImageFile, id, "TvSeries");

                    var updateTvSerie = _mapper.Map<UpdateTvSerieCommand>(command);
                    updateTvSerie.Id = id;
                    updateTvSerie.ImagePath = imagePath;

                    await Mediator.Send(updateTvSerie);
                }

                var tvSerie = await Mediator.Send(new GetTvSerieByIdQuery() { Id = id });

                return Ok(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TvSerieUpdateResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, UpdateTvSerieCommand command)
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
                    var tvSerie = await Mediator.Send(new GetTvSerieByIdQuery() { Id = id });

                    var imagePath = await _fileManagerService.UploadFileAsync(command.ImageFile, id, "TvSeries", tvSerie.ImagePath);
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
                await Mediator.Send(new DeleteTvSerieByIdCommand { Id = id });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
