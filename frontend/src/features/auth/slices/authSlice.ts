import { createAsyncThunk, createSlice, isAnyOf } from "@reduxjs/toolkit";
import type { AuthData, LoginCredentials, RegisterData } from "../types/auth.types";
import { checkAuthStatus, loginUser, logoutUser, registerUser } from "../services/authAPI";

interface AuthState {
    user: AuthData | null;
    isAuthenticated: boolean;
    status: 'idle' | 'loading' | 'succeeded' | 'failed';
    error: string | null;
}

const initialState: AuthState = {
    user: null,
    isAuthenticated: false,
    status: 'idle',
    error: null,
}

export const login = createAsyncThunk('auth/login', async (credentials: LoginCredentials) => {
    return await loginUser(credentials);
});

export const register = createAsyncThunk('auth/register', async (userData: RegisterData) => {
    return await registerUser(userData);
});

export const logout = createAsyncThunk('auth/logout', async () => {
    return await logoutUser()
});

export const checkAuth = createAsyncThunk('auth/checkAuth', async () => {
    return await checkAuthStatus();
})

const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder
            .addCase(logout.fulfilled, (state) => {
                state.user = null;
                state.isAuthenticated = false;
            })
            .addMatcher(isAnyOf(login.fulfilled, register.fulfilled, checkAuth.fulfilled), (state, action) => {
                state.status = 'succeeded';
                state.isAuthenticated = true;
                state.user = action.payload.data;
            })
            .addMatcher(isAnyOf(login.pending, register.pending, checkAuth.pending, logout.pending), (state) => {
                state.status = 'loading';
                state.error = null;
            })
            .addMatcher(isAnyOf(login.rejected, register.rejected, checkAuth.rejected, logout.rejected), (state, action) => {
                state.status = 'failed';
                state.isAuthenticated = false;
                state.user = null;
                state.error = action.error.message || null;
            });
    },
});

export default authSlice.reducer;