import { AxiosError, type AxiosInstance, type AxiosResponse } from 'axios';
import type { BackendResponse, BaseBackendResponse } from '../types/api.types';
import { extractErrorMessage } from '../utils/errorUtils';
import { refreshToken } from '../features/auth/services/authAPI';
import { logout } from '../features/auth/slices/authSlice';

export const setupInterceptors = (axiosInstance: AxiosInstance, store: any) => {
    axiosInstance.interceptors.request.use((config) => {
        const token = store.getState().auth.token;
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    axiosInstance.interceptors.response.use(
        (response: AxiosResponse<BaseBackendResponse>) => {
            const { data } = response;
            if (data.succeeded === false) {
                const errorMessage = extractErrorMessage({ response } as AxiosError<BackendResponse<unknown>>);
                console.error('Backend logic error:', errorMessage);
                const error = new Error(errorMessage);
                throw error;
            }
            return response;
        },
        async (error: AxiosError<BackendResponse<unknown>>) => {
            const originalRequest = error.config;

            if (originalRequest?.url?.includes('/refresh-token') || originalRequest?.url?.includes('/revoke-token')) 
            {
                return Promise.reject(error);
            }

            if (error.response?.status === 401 && originalRequest && !originalRequest._retry) {
                originalRequest._retry = true;
                try {
                    await refreshToken();
                    return axiosInstance(originalRequest);
                } catch (refreshError) {
                    if (!originalRequest?.url?.includes('/me')) {
                        store.dispatch(logout());
                    }
                    return Promise.reject(refreshError);
                }
            }
            const errorMessage = extractErrorMessage(error);
            console.error('API Error:', errorMessage);

            error.message = errorMessage;
            return Promise.reject(error);
        }
    );
};