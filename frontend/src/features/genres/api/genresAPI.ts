import apiClient from "../../../services/apiClient"
import type { BackendResponse } from "../../../types/api.types"
import type { Genre } from "../types/genre.types"

export const fetchGenresAPI = async () => {
    const response = await apiClient.get<BackendResponse<Genre[]>>('/genre');
    return response.data;
};

export const fetchGenreByIdAPI = async (id: number) => {
    const response = await apiClient.get<BackendResponse<Genre>>(`/genre/${id}`);
    return response.data;
};

export const createGenreAPI = async (genre: { name: string })  => {
    const response = await apiClient.post<BackendResponse<number>>(`/genre`, genre);
    return response.data;
};

export const updateGenreAPI = async (id: number, genre: { name: string }) => {
    const response = await apiClient.put<BackendResponse<Genre>>(`/genre/${id}`, genre);
    return response.data;
};

export const deleteGenreAPI = async (id: number) => {
    const response = await apiClient.delete<BackendResponse<number>>(`/genre/${id}`);
    return response.data;
};