import apiClient from '../../../services/api';
import type { BackendResponse } from '../../../types/api.types';
import type { FetchMoviesParams, Movie } from '../types/movie.types';

export const fetchMoviesAPI = async (params?: FetchMoviesParams) => {
    const response = await apiClient.get<BackendResponse<Movie[]>>('/movies', { params });
    return response.data;
};

export const fetchMovieByIdAPI = async (id: number) => {
    const response = await apiClient.get<BackendResponse<Movie>>(`/movies/${id}`);
    return response.data;
};

export const createMovieAPI = async (movieData: FormData) => { 
    const response = await apiClient.post<BackendResponse<number>>('/movies', movieData, {
        headers: {
            'Content-Type': 'multipart/form-data',
        },
    });
    return response.data;
};