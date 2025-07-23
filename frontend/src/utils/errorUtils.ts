import axios, { AxiosError } from 'axios';
import type { BaseBackendResponse } from '../types/api.types';

export type THttpError = Error | AxiosError<BaseBackendResponse> | null;

export function extractErrorMessage(error: THttpError): string {
    if (axios.isAxiosError(error)) {
        const axiosError = error as AxiosError<BaseBackendResponse>;
        const backendData = axiosError.response?.data;

        if (backendData) {
            if (backendData.error?.errors?.length) {
                return backendData.error.errors.join(', ');
            }
            if (backendData.error?.message) {
                return backendData.error.message;
            }
            if (backendData.message) {
                return backendData.message;
            }
        }

        if (axiosError.response?.statusText) {
            return `Server error: ${axiosError.response.status}: ${axiosError.response.statusText}`;
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