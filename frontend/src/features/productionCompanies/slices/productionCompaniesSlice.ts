import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { createProductionCompanyAPI, deleteProductionCompanyAPI, fetchProductionCompaniesAPI, fetchProductionCompanyByIdAPI, updateProductionCompanyAPI } from "../api/productionCompaniesAPI";
import type { ProductionCompany } from "../types/productionCompany.types";
import type { RootState } from "../../../app/store";

interface ProductionCompaniesState {
    productionCompanies: ProductionCompany[];
    selectedProductionCompany: ProductionCompany | null;
    status: 'idle' | 'loading' | 'succeeded' | 'failed';
    error: string | null;
}

const initialState: ProductionCompaniesState = {
    productionCompanies: [],
    selectedProductionCompany: null,
    status: 'idle',
    error: null,
};

export const fetchProductionCompaniesAsync = createAsyncThunk(
    'productionCompanies/fetchProductionCompanies',
    async () => {
        const response = await fetchProductionCompaniesAPI();
        return response.data;
    }
);

export const fetchProductionCompanyByIdAsync = createAsyncThunk(
    'productionCompanies/fetchProductionCompanyById',
    async (id: number) => {
        const response = await fetchProductionCompanyByIdAPI(id);
        return response.data;
    }
);

export const createProductionCompanyAsync = createAsyncThunk(
    'productionCompanies/createProductionCompany',
    async (company: { name: string }) => {
        const response = await createProductionCompanyAPI(company);
        return response.data;
    }
);

export const updateProductionCompanyAsync = createAsyncThunk(
    'productionCompanies/updateProductionCompany',
    async (company: { id: number, name: string }) => {
        const response = await updateProductionCompanyAPI(company.id, { name: company.name });
        return response.data;
    }
);

export const deleteProductionCompanyAsync = createAsyncThunk(
    'productionCompanies/deleteProductionCompany',
    async (id: number) => {
        await deleteProductionCompanyAPI(id);
        return id;
    }
);

const productionCompaniesSlice = createSlice({
    name: 'productionCompanies',
    initialState,
    reducers: {
        clearSelectedProductionCompany: (state) => {
            state.selectedProductionCompany = null;
        }
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchProductionCompaniesAsync.pending, (state) => {
                state.status = 'loading';
            })
            .addCase(fetchProductionCompaniesAsync.fulfilled, (state, action) => {
                state.status = 'succeeded';
                state.productionCompanies = action.payload;
            })
            .addCase(fetchProductionCompaniesAsync.rejected, (state, action) => {
                state.status = 'failed';
                state.error = action.error.message ?? 'Something went wrong';
            })
            .addCase(fetchProductionCompanyByIdAsync.pending, (state) => {
                state.status = 'loading';
            })
            .addCase(fetchProductionCompanyByIdAsync.fulfilled, (state, action) => {
                state.status = 'succeeded';
                state.selectedProductionCompany = action.payload;
            })
            .addCase(fetchProductionCompanyByIdAsync.rejected, (state, action) => {
                state.status = 'failed';
                state.error = action.error.message ?? 'Something went wrong';
            })
            .addCase(createProductionCompanyAsync.fulfilled, (state) => {
                state.status = 'succeeded';
                state.error = null;
            })
            .addCase(updateProductionCompanyAsync.fulfilled, (state, action) => {
                const index = state.productionCompanies.findIndex(company => company.id === action.payload.id);
                if (index !== -1) {
                    state.productionCompanies[index] = action.payload;
                }
            })
            .addCase(deleteProductionCompanyAsync.fulfilled, (state, action) => {
                state.productionCompanies = state.productionCompanies.filter(company => company.id !== action.payload);
            });
    },
});

export const { clearSelectedProductionCompany } = productionCompaniesSlice.actions;
export const selectProductionCompanies = (state: RootState) => state.productionCompanies.productionCompanies;
export const selectProductionCompaniesStatus = (state: RootState) => state.productionCompanies.status;
export const selectProductionCompaniesError = (state: RootState) => state.productionCompanies.error;
export const selectSelectedProductionCompany = (state: RootState) => state.productionCompanies.selectedProductionCompany;

export default productionCompaniesSlice.reducer;