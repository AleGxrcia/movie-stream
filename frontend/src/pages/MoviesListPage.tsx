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
  const [pageSize, setpageSize] = useState(16);

  useEffect(() => {
    dispatch(fetchMoviesAsync({ pageNumber: currentPage, pageSize: pageSize }));
  }, [dispatch, currentPage]);
  
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
    <div className="mx-auto px-0 py-8 w-[calc(100%-3%)] max-w-[1800px]">
      <h1 className="text-3xl font-bold mb-8 px-[4%]">Películas</h1>
      <div className="flex flex-wrap justify-center">
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