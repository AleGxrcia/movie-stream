﻿using AutoMapper;
using MediatR;
using MovieStream.Core.Application.Common.Parameters.Base;
using MovieStream.Core.Application.DTOs.Genre;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Core.Application.Wrappers;
using System.Net;

namespace MovieStream.Core.Application.Features.Genres.Queries.GetAllGenres
{
    public class GetAllGenresQuery : IRequest<Response<List<GenreDto>>>
    {
        public RequestParameters? Parameters { get; set; }
    }

    public class GetAllGenresQueryHandler : IRequestHandler<GetAllGenresQuery, Response<List<GenreDto>>>
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GetAllGenresQueryHandler(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<GenreDto>>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
        {
            var filters = request.Parameters;
            var pagedGenresDto = await GetAllDtoWithFilters(filters);
            if (pagedGenresDto == null || pagedGenresDto.Count == 0) throw new ApiException("Genres not found.", (int)HttpStatusCode.NotFound);

            return new Response<List<GenreDto>>(pagedGenresDto);
        }

        private async Task<List<GenreDto>> GetAllDtoWithFilters(RequestParameters parameters)
        {
            var genreList = await _genreRepository.GetAllAsync();
            var genreDtoList = _mapper.Map<List<GenreDto>>(genreList);

            return genreDtoList;
        }
    }
}
