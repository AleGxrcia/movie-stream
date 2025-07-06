import apiClient from '../../../services/api';
import type { PaginatedResponse } from '../../../types/pagination.types';
import type { FetchMoviesParams, Movie } from '../types/movie.types';

export const fetchMoviesAPI = async (params?: FetchMoviesParams): Promise<PaginatedResponse<Movie>> => {
    try {
        const response = await apiClient.get<PaginatedResponse<Movie>>('/movies', { params });
        return response.data;
    } catch (error: any) {
        console.error('Error fetching movies:', error.response?.data || error.message);
        throw error.response?.data || new Error('Failed to fetch movies');
    }
};

export const fetchMovieByIdAPI = async (id: number): Promise<Movie> => {
    try {
        const response = await apiClient.get<Movie>(`/movies/${id}`);
        return response.data;
    } catch (error: any) {
        console.error(`Error fetching movie with id ${id}:`, error.response?.data || error.message);
        throw error.response?.data || new Error(`Failed to fetch movie with id ${id}`);
    }
};

export const createMovieAPI = async (movieData: FormData): Promise<number> => { 
    try {
        const response = await apiClient.post<number>('/movies', movieData, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        });
        return response.data; 
    } catch (error: any) {
        console.error('Error creating movie:', error.response?.data || error.message);
        throw error.response?.data || new Error('Failed to create movie');
    }
};