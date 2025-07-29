import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { fetchProductionCompaniesAPI } from "../api/productionCompaniesAPI";
import type { ProductionCompany } from "../types/productionCompany.types";
import type { RootState } from "../../../app/store";

interface ProductionCompaniesState {
    productionCompanies: ProductionCompany[];
    status: 'idle' | 'loading' | 'succeeded' | 'failed';
    error: string | null;
}

const initialState: ProductionCompaniesState = {
    productionCompanies: [],
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

const productionCompaniesSlice = createSlice({
    name: 'productionCompanies',
    initialState,
    reducers: {},
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
            });
    },
});

export const selectProductionCompanies = (state: RootState) => state.productionCompanies.productionCompanies;
export const selectProductionCompaniesStatus = (state: RootState) => state.productionCompanies.status;

export default productionCompaniesSlice.reducer;
