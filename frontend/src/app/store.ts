import { configureStore } from "@reduxjs/toolkit";
import moviesReducer from '../features/movies/slices/moviesSlice';
import authReducer from '../features/auth/slices/authSlice';

export const store = configureStore({
    reducer: {
        movies: moviesReducer,
        auth: authReducer,
    },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;