import { AxiosError, type AxiosInstance, type AxiosResponse } from 'axios';
import type { BackendResponse, BaseBackendResponse } from '../types/api.types';
import { extractErrorMessage } from '../utils/errorUtils';

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
        (error: AxiosError<BackendResponse<unknown>>) => {
            const errorMessage = extractErrorMessage(error);
            console.error('API Error:', errorMessage);

            if (error.response?.status === 401) {
                const { logout } = require('../features/auth/slices/authSlice');
                store.dispatch(logout());
            }

            error.message = errorMessage;
            return Promise.reject(error);
        }
    );
};