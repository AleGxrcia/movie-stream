import apiClient from "../../../services/api";
import type { AuthResponse, LoginCredentials, RegisterData } from "../types/auth.types";

export const loginUser = async (credentials: LoginCredentials): Promise<AuthResponse> => {
    const response = await apiClient.post<AuthResponse>('/account/authenticate', credentials);
    return response.data;
};

export const registerUser = async (userData: RegisterData): Promise<AuthResponse> => {
    const response = await apiClient.post<AuthResponse>('/account/resgister', userData);
    return response.data;
};