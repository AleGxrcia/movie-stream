import apiClientAuth from "../../../services/apiAuth";
import type { AuthResponse, LoginCredentials, RegisterData } from "../types/auth.types";

export const loginUser = async (credentials: LoginCredentials) => {
    const response = await apiClientAuth.post<AuthResponse>('/account/authenticate', credentials);
    return response.data;
};

export const registerUser = async (userData: RegisterData) => {
    const response = await apiClientAuth.post<AuthResponse>('/account/register', userData);
    return response.data;
};

export const logoutUser = async () => {
    const response = await apiClientAuth.post('/account/revoke-token');
    return response.data;
};

export const checkAuthStatus = async () => {
    const response = await apiClientAuth.get<AuthResponse>('/account/me');
    return response.data;
};

export const refreshToken = async () => {
    const response = await apiClientAuth.post<AuthResponse>('/account/refresh-token');
    return response.data;
};