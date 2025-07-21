import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { fetchMoviesAPI } from "../api/moviesAPI";
import type { Movie } from "../types/movie.types";
import type { RootState } from "../../../app/store";


interface MoviesState {
    movies: Movie[];
    status: 'idle' | 'loading' | 'succeeded' | 'failed';
    error: string | null;
}

const initialState: MoviesState = {
    movies: [],
    status: 'idle',
    error: null,
};

export const fetchMoviesAsync = createAsyncThunk(
    'movies/fetchMovies',
    async () => {
        const response = await fetchMoviesAPI();
        return response.data;
    }
);

const moviesSlice = createSlice({
    name: 'movies',
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder
            .addCase(fetchMoviesAsync.pending, (state) => {
                state.status = 'loading';
            })
            .addCase(fetchMoviesAsync.fulfilled, (state, action) => {
                state.status = 'succeeded';
                state.movies = action.payload;
            })
            .addCase(fetchMoviesAsync.rejected, (state, action) => {
                state.status = 'failed';
                state.error = action.error.message ?? 'Something went wrong';
            });
    },
});

export const selectMovies = (state: RootState) => state.movies.movies;
export const selectMoviesStatus = (state: RootState) => state.movies.status;
export const selectMoviesError = (state: RootState) => state.movies.error;

export default moviesSlice.reducer;