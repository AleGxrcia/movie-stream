import { configureStore } from "@reduxjs/toolkit";
import moviesReducer from '../features/movies/slices/moviesSlice';
import authReducer from '../features/auth/slices/authSlice';
import genresReducer from '../features/genres/slices/genreSlice';
import productionCompaniesReducer from '../features/productionCompanies/slices/productionCompaniesSlice';

export const store = configureStore({
    reducer: {
        movies: moviesReducer,
        auth: authReducer,
        genres: genresReducer,
        productionCompanies: productionCompaniesReducer,
    },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;