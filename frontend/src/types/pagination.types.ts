export interface MetaData {
    currentPage: number;
    totalPages: number;
    pageSize: number;
    totalCount: number;
    hasNextPage?: boolean;
    hasPreviousPage?: boolean;
}

export interface PagedList<T> {
    items: T[];
    metaData: MetaData;
}