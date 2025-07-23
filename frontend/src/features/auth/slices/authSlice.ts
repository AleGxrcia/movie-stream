import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import type { AuthResponse, LoginCredentials, RegisterData } from "../types/auth.types";
import { loginUser, registerUser } from "../services/authAPI";

interface AuthState {
    user: AuthResponse | null;
    token: string | null;
    isAuthenticated: boolean;
    status: 'idle' | 'loading' | 'succeeded' | 'failed';
    error: string | null;
}

const initialState: AuthState = {
    user: null,
    token: localStorage.getItem('token'),
    isAuthenticated: !!localStorage.getItem('token'),
    status: 'idle',
    error: null,
}

export const login = createAsyncThunk('auth/login', async (credentials: LoginCredentials) => {
    const response = await loginUser(credentials);
    localStorage.setItem('token', response.token);
    return response;
});

export const register = createAsyncThunk('auth/register', async (userData: RegisterData) => {
    const response = await registerUser(userData);
    localStorage.setItem('token', response.token);
    return response;
});

const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        logout: (state) => {
            localStorage.removeItem('token');
            state.user = null;
            state.token = null;
            state.isAuthenticated = false;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(login.pending, (state) => {
                state.status = 'loading';
            })
            .addCase(login.fulfilled, (state, action) => {
                state.status = 'succeeded';
                state.isAuthenticated = true;
                state.user = action.payload;
                state.token = action.payload.token;
            })
            .addCase(login.rejected, (state, action) => {
                state.status = 'failed';
                state.error = action.error.message || null;
            })
            .addCase(register.pending, (state) => {
                state.status = 'loading';
            })
            .addCase(register.fulfilled, (state, action) => {
                state.status = 'succeeded';
                state.isAuthenticated = true;
                state.user = action.payload;
                state.token = action.payload.token;
            })
            .addCase(register.rejected, (state, action) => {
                state.status = 'failed';
                state.error = action.error.message || null;
            });
    },
});

export const { logout } = authSlice.actions;

export default authSlice.reducer;