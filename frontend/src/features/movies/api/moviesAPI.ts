import apiClient from '../../../services/apiClient';
import type { BackendResponse } from '../../../types/api.types';
import type { FetchMoviesParams, Movie } from '../types/movie.types';

export const fetchMoviesAPI = async (params?: FetchMoviesParams) => {
    const response = await apiClient.get<BackendResponse<Movie[]>>('/movie', { params });
    return response.data;
};

export const fetchMovieByIdAPI = async (id: number) => {
    const response = await apiClient.get<BackendResponse<Movie>>(`/movie/${id}`);
    return response.data;
};

export const createMovieAPI = async (movieData: FormData) => { 
    const response = await apiClient.post<BackendResponse<number>>('/movie', movieData, {
        headers: {
            'Content-Type': 'multipart/form-data',
        },
    });
    return response.data;
};