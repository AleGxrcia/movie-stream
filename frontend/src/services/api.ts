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
    (response: AxiosResponse) => {

        const data = response.data as BackendResponse<unknown>;
        if (data && data.succeeded === false) {
            console.error('Backend logic error:', data.message, data.errors);
            const error = new Error(extractErrorMessage({ response: { data } } as AxiosError<BackendResponse<any>>));
            throw error;
        }
        return response.data;
    },
    (error: AxiosError<BackendResponse<any>>) => {
        const errorMessage = extractErrorMessage(error);
        console.error('API Error:', errorMessage);

        error.message = errorMessage;
        throw error;
    }
);

export default apiClient;