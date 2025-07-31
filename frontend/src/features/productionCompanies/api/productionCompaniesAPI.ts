import apiClient from "../../../services/apiClient";
import type { BackendResponse } from "../../../types/api.types";
import type { ProductionCompany } from "../types/productionCompany.types";

export const fetchProductionCompaniesAPI = async () => {
    const response = await apiClient.get<BackendResponse<ProductionCompany[]>>('/productioncompany');
    return response.data;
};

export const fetchProductionCompanyByIdAPI = async (id: number) => {
    const response = await apiClient.get<BackendResponse<ProductionCompany>>(`/productionCompany/${id}`);
    return response.data;
};

export const createProductionCompanyAPI = async (company: { name: string }) => {
    const response = await apiClient.post<BackendResponse<number>>(`/productionCompany`, company);
    return response.data;
};

export const updateProductionCompanyAPI = async (id: number, company: { name: string }) => {
    const response = await apiClient.put<BackendResponse<ProductionCompany>>(`/productionCompany/${id}`, company);
    return response.data;
};

export const deleteProductionCompanyAPI = async (id: number) => {
    const response = await apiClient.delete<BackendResponse<number>>(`/productionCompany/${id}`);
    return response.data;
};