import { useEffect, useState } from "react"
import type { Movie } from "../features/movies/types/movie.types"
import { fetchMoviesAPI } from "../features/movies/api/moviesAPI";
import { extractErrorMessage, type THttpError } from "../utils/errorUtils";
import MovieCard from "../features/movies/components/MovieCard";
import type { AppDispatch } from "../app/store";
import { useDispatch, useSelector } from "react-redux";
import { fetchMoviesAsync, selectMovies, selectMoviesError, selectMoviesStatus } from "../features/movies/slices/moviesSlice";

const MoviesListPage = () => {
  const dispatch = useDispatch<AppDispatch>();
  const movies = useSelector(selectMovies);
  const status = useSelector(selectMoviesStatus);
  const error = useSelector(selectMoviesError);

    useEffect(() => {
      if (status === 'idle') {
        dispatch(fetchMoviesAsync());
      }
    }, [status, dispatch]);
    
    if (status === 'loading') {
        return <div className="text-center">Cargando...</div>;
    }

    if (status === 'failed') {
        return <div className="text-center text-red-500">Error: {error}</div>;
    }

    return (
      <div className="container mx-auto px-4 py-8">
        <h1 className="text-3xl font-bold mb-8">Películas</h1>
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-8">
          {movies.map((movie) => (
            <MovieCard key={movie.id} movie={movie} />
          ))}
        </div>
      </div>
    );
};

export default MoviesListPage;
