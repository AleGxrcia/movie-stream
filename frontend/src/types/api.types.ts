export interface BackendResponse<T> {
    succeeded: boolean;
    message: string | null;
    errors?: string[] | null;
    data: T;
}

export interface ApiException {
    errorCode: number;
    message: string;
}