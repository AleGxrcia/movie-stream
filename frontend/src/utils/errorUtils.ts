import axios, { AxiosError } from 'axios';
import type { BackendResponse } from '../types/api.types';

export type THttpError = Error | AxiosError<BackendResponse<unknown>> | null;

export function extractErrorMessage(error: THttpError): string {
    if (axios.isAxiosError(error)) {
        const axiosError = error as AxiosError<BackendResponse<unknown>>;

        if (axiosError.response) {
            const backendData = axiosError.response.data;

            if (backendData.message) {
                let message = backendData.message;
                if (backendData.errors?.length) {
                    message += ` Details: ${backendData.errors.join(', ')}`;
                }
                return message;
            }
            if (axiosError.response.statusText) {
                return `Server error: ${axiosError.response.status}: ${axiosError.response.statusText}`;
            }
        }
        
        if (axiosError.request && !axiosError.response) {
            return 'Network error: Unable to connect to the server. Please try again later.';
        }
        return axiosError.message || 'An error occurred while sending the request.';
    }

    if (error instanceof Error) {
        return error?.message || 'An unexpected error occurred.';
    }
    
    return 'An unknown error occurred. Please try again.';
}