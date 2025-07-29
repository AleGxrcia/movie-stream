import apiClient from "../../../services/apiClient";
import type { BackendResponse } from "../../../types/api.types";
import type { ProductionCompany } from "../types/productionCompany.types";

export const fetchProductionCompaniesAPI = async () => {
    const response = await apiClient.get<BackendResponse<ProductionCompany[]>>('/productioncompany');
    return response.data;
};