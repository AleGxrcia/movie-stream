import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import type { Genre } from "../types/genre.types";
import { fetchGenresAPI } from "../api/genresAPI";
import type { RootState } from "../../../app/store";

interface GenreState {
    genres: Genre[];
    status: 'idle' | 'loading' | 'succeeded' | 'failed';
    error: string | null
}

const initialState: GenreState = {
    genres: [],
    status: 'idle',
    error: null,
};

export const fetchGenresAsync = createAsyncThunk(
    'genres/fetchGenres',
    async () => {
        const response = await fetchGenresAPI();
        return response.data;
    }
);

const genresSlice = createSlice({
    name: 'genres',
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder
            .addCase(fetchGenresAsync.pending, (state) => {
                state.status = 'loading';
            })
            .addCase(fetchGenresAsync.fulfilled, (state, action) => {
                state.status = 'succeeded';
                state.genres = action.payload;
            })
            .addCase(fetchGenresAsync.rejected, (state, action) => {
                state.status = 'failed';
                state.error = action.error.message ?? 'Something went wrong';
            });
    },
});

export const selectGenres = (state:RootState) => state.genres.genres;
export const selectGenresStatus = (state: RootState) => state.genres.status;

export default genresSlice.reducer;