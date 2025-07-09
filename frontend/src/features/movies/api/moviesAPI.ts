import apiClient from '../../../services/api';
import type { BackendResponse } from '../../../types/api.types';
import type { PaginatedResponse } from '../../../types/pagination.types';
import type { FetchMoviesParams, Movie } from '../types/movie.types';

export const fetchMoviesAPI = async (params?: FetchMoviesParams): Promise<PaginatedResponse<Movie>> => {
    const response = await apiClient.get<PaginatedResponse<Movie>>('/movies', { params });
    return response.data;
};

export const fetchMovieByIdAPI = async (id: number): Promise<Movie> => {
    const response = await apiClient.get<BackendResponse<Movie>>(`/movies/${id}`);
    return response.data.data;
};

export const createMovieAPI = async (movieData: FormData): Promise<number> => { 
    const response = await apiClient.post<BackendResponse<number>>('/movies', movieData, {
        headers: {
            'Content-Type': 'multipart/form-data',
        },
    });
    return response.data.data;
};