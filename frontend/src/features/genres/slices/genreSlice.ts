import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import type { Genre } from "../types/genre.types";
import { createGenreAPI, deleteGenreAPI, fetchGenreByIdAPI, fetchGenresAPI, updateGenreAPI } from "../api/genresAPI";
import type { RootState } from "../../../app/store";

interface GenreState {
    genres: Genre[];
    selectedGenre: Genre | null;
    status: 'idle' | 'loading' | 'succeeded' | 'failed';
    error: string | null
}

const initialState: GenreState = {
    genres: [],
    selectedGenre: null,
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

export const fetchGenreByIdAsync = createAsyncThunk(
    'genres/fetchGenreById',
    async (id: number) => {
        const response = await fetchGenreByIdAPI(id);
        return response.data;
    }
);

export const createGenreAsync = createAsyncThunk(
    'genres/createGenre',
    async (genre: { name: string }) => {
        const response = await createGenreAPI(genre);
        return response.data;
    }
);

export const updateGenreAsync = createAsyncThunk(
    'genres/updateGenre',
    async (genre: { id: number, name: string }) => {
        const response = await updateGenreAPI(genre.id, { name: genre.name });
        return response.data;
    }
);

export const deleteGenreAsync = createAsyncThunk(
    'genres/deleteGenre',
    async (id: number) => {
        await deleteGenreAPI(id);
        return id;
    }
);

const genresSlice = createSlice({
    name: 'genres',
    initialState,
    reducers: {
        clearSelectedGenre: (state) => {
            state.selectedGenre = null;
        }
    },
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
            })
            .addCase(fetchGenreByIdAsync.pending, (state) => {
                state.status = 'loading';
            })
            .addCase(fetchGenreByIdAsync.fulfilled, (state, action) => {
                state.status = 'succeeded';
                state.selectedGenre = action.payload;
            })
            .addCase(fetchGenreByIdAsync.rejected, (state, action) => {
                state.status = 'failed';
                state.error = action.error.message ?? 'Something went wrong';
            })
            .addCase(createGenreAsync.fulfilled, (state) => {
                state.status = 'succeeded';
                state.error = null;
            })
            .addCase(updateGenreAsync.fulfilled, (state, action) => {
                const index = state.genres.findIndex(genre => genre.id === action.payload.id);
                if (index !== -1) {
                    state.genres[index] = action.payload;
                }
            })
            .addCase(deleteGenreAsync.fulfilled, (state, action) => {
                state.genres = state.genres.filter(genre => genre.id !== action.payload);
            });
    },
});

export const { clearSelectedGenre } = genresSlice.actions;
export const selectGenres = (state: RootState) => state.genres.genres;
export const selectGenresStatus = (state: RootState) => state.genres.status;
export const selectGenresError = (state: RootState) => state.genres.error;
export const selectSelectedGenre = (state: RootState) => state.genres.selectedGenre;

export default genresSlice.reducer;