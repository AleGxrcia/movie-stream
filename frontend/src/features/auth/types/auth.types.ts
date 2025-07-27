export interface LoginCredentials {
    email?: string;
    password?: string;
}

export interface RegisterData {
    firstName?: string;
    lastName?: string;
    email?: string;
    userName?: string;
    password?: string;
    confirmPassword?: string;
    phoneNumber?: string;
}

export interface AuthData {
    id: string;
    userName: string;
    email: string;
    roles: string[];
    isVerified: boolean;
    jwToken: string;
    refreshToken: string;
}

export interface AuthResponse {
    data: AuthData;
    succeeded: boolean;
    message: string;
    error?: string | null;
}