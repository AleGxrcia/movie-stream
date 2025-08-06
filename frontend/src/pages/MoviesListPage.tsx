import { useEffect, useState } from "react"
import MovieCard from "../features/movies/components/MovieCard";
import { useAppDispatch, useAppSelector } from "../app/hooks";
import { fetchMoviesAsync, selectMovies, selectMoviesError, selectMoviesMetaData, selectMoviesStatus } from "../features/movies/slices/moviesSlice";
import Pagination from "../components/common/Pagination";

const MoviesListPage = () => {
  const dispatch = useAppDispatch();
  const movies = useAppSelector(selectMovies);
  const metaData = useAppSelector(selectMoviesMetaData);
  const status = useAppSelector(selectMoviesStatus);
  const error = useAppSelector(selectMoviesError);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(12);

  useEffect(() => {
    dispatch(fetchMoviesAsync({ pageNumber: currentPage, pageSize: pageSize }));
  }, [dispatch, currentPage, pageSize]);
  
  const handlePageChange = (newPage: number) => {
    setCurrentPage(newPage);
  };

  if (status === 'loading') {
    return <div className="text-center">Cargando...</div>;
  }

  if (status === 'failed') {
    return <div className="text-center text-red-500">Error: {error}</div>;
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold mb-8">Películas</h1>
      <div className="flex flex-wrap justify-center md:justify-start -mx-2">
        {movies.map((movie) => (
          <MovieCard key={movie.id} movie={movie} />
        ))}
      </div>
      {metaData && (
        <Pagination
          currentPage={metaData.currentPage}
          totalPages={metaData.totalPages}
          onPageChange={handlePageChange}
          hasNextPage={metaData.hasNextPage}
          hasPreviousPage={metaData.hasPreviousPage}
          totalItems={metaData.totalCount}
          pageSize={metaData.pageSize}
        />
      )}
    </div>
  );
};

export default MoviesListPage;