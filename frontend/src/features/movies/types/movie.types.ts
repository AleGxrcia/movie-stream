import type { Genre } from '../../genres/types/genre.types';
import type { ProductionCompany } from '../../productionCompanies/types/productionCompany.types';

export interface Movie {
    id: number;
    name: string;
    description: string;
    imageUrl: string;
    runtime: string;
    releaseDate: string;
    genres?: Genre[];
    productionCompany: ProductionCompany;
}

export interface FetchMoviesParams {
    sortColum?: string;
    sortOrder?: 'asc' | 'desc';
    filterBy?: string;
    filterValue?: string;
    pageNumber?: number;
    pageSize?: number;
}