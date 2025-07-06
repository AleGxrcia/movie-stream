export interface MetaData {
    pageNumber: number;
    pageSize: number;
    totalItems: number;
    totalpages: number;
    hasNextPage: boolean;
    hasPreviousPage: boolean;
}

export interface PaginatedResponse<T> {
    data: T[];
    meta: MetaData;
}