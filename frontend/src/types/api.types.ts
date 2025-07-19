import type { MetaData } from "./pagination.types";

export interface BackendResponse<T> {
    succeeded: boolean;
    message: string | null;
    error?: Error | null;
    data: T;
    meta?: MetaData | null;
}

export interface Error {
    code: number;
    message: string;
    errors?: string[] | null;
}