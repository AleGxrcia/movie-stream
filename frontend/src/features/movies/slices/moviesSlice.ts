import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { createMovieAPI, deleteMovieAPI, fetchMovieByIdAPI, fetchMoviesAPI, updateMovieAPI } from "../api/moviesAPI";
import type { FetchMoviesParams, Movie } from "../types/movie.types";
import type { RootState } from "../../../app/store";
import type { MetaData } from "../../../types/pagination.types";


interface MoviesState {
    movies: Movie[];
    selectedMovie: Movie | null;
    metaData: MetaData | null;
    status: 'idle' | 'loading' | 'succeeded' | 'failed';
    error: string | null;
}

const initialState: MoviesState = {
    movies: [],
    selectedMovie: null,
    metaData: null,
    status: 'idle',
    error: null,
};

export const fetchMoviesAsync = createAsyncThunk(
    'movies/fetchMovies',
    async (params: FetchMoviesParams) => {
        const response = await fetchMoviesAPI(params);
        return response;
    }
);

export const fetchMovieByIdAsync = createAsyncThunk(
    'movies/fetchMovieById',
    async (id: number) => {
        const response = await fetchMovieByIdAPI(id);
        return response.data;
    }
);

export const createMovieAsync = createAsyncThunk(
    'movies/createMovie',
    async (movieData: FormData) => {
        const response = await createMovieAPI(movieData);
        const newMovieId = response.data;
        const newMovieResponse = await fetchMovieByIdAPI(newMovieId);
        return newMovieResponse.data;
    }
);

export const updateMovieAsync = createAsyncThunk(
    'movies/updateMovie',
    async ({ id, movieData }: { id: number, movieData: FormData }) => {
        const response = await updateMovieAPI(id, movieData);
        return response.data;
    }
);

export const deleteMovieAsync = createAsyncThunk(
    'movies/deleteMovie',
    async (id: number) => {
        const response = await deleteMovieAPI(id);
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
                state.movies = action.payload.data;
                state.metaData = action.payload.meta ?? null;
            })
            .addCase(fetchMoviesAsync.rejected, (state, action) => {
                state.status = 'failed';
                state.error = action.error.message ?? 'Something went wrong';
            })
            .addCase(fetchMovieByIdAsync.pending, (state) => {
                state.status = 'loading';
            })
            .addCase(fetchMovieByIdAsync.fulfilled, (state, action) => {
                state.status = 'succeeded';
                state.selectedMovie = action.payload;
            })
            .addCase(fetchMovieByIdAsync.rejected, (state, action) => {
                state.status = 'failed';
                state.error = action.error.message ?? 'Something went wrong';
            })
            .addCase(createMovieAsync.fulfilled, (state, action) => {
                state.movies.push(action.payload);
            })
            .addCase(updateMovieAsync.fulfilled, (state, action) => {
                const index = state.movies.findIndex((movie) => movie.id === action.payload.id);
                if (index !== -1) {
                    state.movies[index] = action.payload;
                }
            })
            .addCase(deleteMovieAsync.fulfilled, (state, action) => {
                state.movies = state.movies.filter((movie) => movie.id !== action.payload);
            });
    },
});

export const selectMovies = (state: RootState) => state.movies.movies;
export const selectMoviesStatus = (state: RootState) => state.movies.status;
export const selectMoviesError = (state: RootState) => state.movies.error;
export const selectSelectedMovie = (state: RootState) => state.movies.selectedMovie;
export const selectMoviesMetaData = (state: RootState) => state.movies.metaData;

export default moviesSlice.reducer;