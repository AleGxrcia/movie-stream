import axios, { AxiosError, type AxiosResponse } from 'axios';
import { API_BASE_URL } from '../constants/api';
import type { BackendResponse } from '../types/api.types';
import { extractErrorMessage } from '../utils/errorUtils';


const apiClient = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

apiClient.interceptors.response.use(
    (response: AxiosResponse<BackendResponse<unknown>>) => {
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

        error.message = errorMessage;
        return Promise.reject(error);
    }
);

export default apiClient;