import { useEffect } from "react"
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
    <div className="mx-auto px-0 py-8 w-[calc(100%-3%)] max-w-[1800px]">
      <h1 className="text-3xl font-bold mb-8 px-[4%]">Películas</h1>
      <div className="flex flex-wrap justify-center">
        {movies.map((movie) => (
          <MovieCard key={movie.id} movie={movie} />
        ))}
      </div>
    </div>
  );
};

export default MoviesListPage;