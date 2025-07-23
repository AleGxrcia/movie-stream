export interface LoginCredentials {
    email?: string;
    password?: string;
}

export interface RegisterData {
    email?: string;
    userName?: string;
    password?: string;
    confirmPassword?: string;
}

export interface AuthResponse {
    id: string;
    userName: string;
    email: string;
    roles: string[];
    isAuthenticated: boolean;
    token: string;
    refreshToken: string;
}