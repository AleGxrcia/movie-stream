import type { MetaData } from "./pagination.types";

export interface BaseBackendResponse {
    succeeded: boolean;
    message: string | null;
    error?: Error | null;
}

export interface BackendResponse<T> extends BaseBackendResponse {
    data: T;
    meta?: MetaData | null;
}

export interface Error {
    code: number;
    message: string;
    errors?: string[] | null;
}