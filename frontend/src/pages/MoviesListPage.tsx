import { useEffect, useState } from "react"
import type { Movie } from "../features/movies/types/movie.types"
import { fetchMoviesAPI } from "../features/movies/api/moviesAPI";
import { extractErrorMessage, type THttpError } from "../utils/errorUtils";
import MovieCard from "../features/movies/components/MovieCard";


const MoviesListPage = () => {
    const [movies, setMovies] = useState<Movie[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
      const fetchMovies = async () => {
        try {
          const paginatedMoviesData = await fetchMoviesAPI();

          if (!paginatedMoviesData || !paginatedMoviesData.data) {
            throw new Error('Respuesta inválida del servidor');
          }

          setMovies(paginatedMoviesData.data);
          setError(null);
        } catch (err) {
          console.error('❌ Error en fetchMovies:', err);
          setError(extractErrorMessage(err as THttpError));
        } finally {
          setLoading(false);
        }
      };

      fetchMovies();
    }, []);
    
    if (loading) {
        return <div className="text-center">Cargando...</div>;
    }

    if (error) {
        return <div className="text-center text-red-500">Error: {error}</div>;
    }

    return (
      <div className="container mx-auto px-4 py-8">
        <h1 className="text-3xl font-bold mb-8">Películas</h1>
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-8">
          {movies.map(movie => (
            <MovieCard key={movie.id} movie={movie} />
          ))}
        </div>

      </div>
    );
};

export default MoviesListPage;
