import apiClientAuth from "../../../services/apiAuth";
import type { AuthResponse, LoginCredentials, RegisterData } from "../types/auth.types";

export const loginUser = async (credentials: LoginCredentials): Promise<AuthResponse> => {
    const response = await apiClientAuth.post<AuthResponse>('/account/authenticate', credentials);
    return response.data;
};

export const registerUser = async (userData: RegisterData): Promise<AuthResponse> => {
    const response = await apiClientAuth.post<AuthResponse>('/account/register', userData);
    return response.data;
};