import apiClient from "../../../services/apiClient"
import type { BackendResponse } from "../../../types/api.types"
import type { Genre } from "../types/genre.types"

export const fetchGenresAPI = async () => {
    const response = await apiClient.get<BackendResponse<Genre[]>>('/genre');
    return response.data;
};