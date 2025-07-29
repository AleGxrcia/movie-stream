import { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../app/hooks";
import { deleteMovieAsync, fetchMoviesAsync, selectMovies, selectMoviesError, selectMoviesStatus } from "../features/movies/slices/moviesSlice";
import { Link, useNavigate } from "react-router-dom";

const ManageMoviesPage = () => {
    const dispatch = useAppDispatch();
    const movies = useAppSelector(selectMovies);
    const status = useAppSelector(selectMoviesStatus);
    const error = useAppSelector(selectMoviesError);
    const navigate = useNavigate();

    useEffect(() => {
        if (status === 'idle') {
            dispatch(fetchMoviesAsync());
        }
    }, [status, dispatch]);

    const handleDelete = (id: number) => {
        if (window.confirm('¿Estás seguro de que deseas eliminar esta película?')) {
            dispatch(deleteMovieAsync(id));
            navigate('/manage-movies');
        }
    };

    if (status === 'loading') {
        return <div className="text-center">Cargando...</div>;
    }

    if (status === 'failed') {
        return <div className="text-center text-red-500">Error: {error}</div>;
    }

    return (
        <div className="container mx-auto px-4 py-8">
            <div className="flex justify-between items-center mb-8">
                <h1 className="text-3xl font-bold">Gestionar Películas</h1>
                <Link to="/movies/create" className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">
                    Crear Película
                </Link>
            </div>
            <div className="shadow-lg rounded-lg overflow-x-auto">
                <table className="w-full table-auto">
                    <thead className="bg-gray-200">
                        <tr>
                            <th className="px-4 py-2">Nombre</th>
                            <th className="px-4 py-2">Fecha de Lanzamiento</th>
                            <th className="px-4 py-2">Acciones</th>
                        </tr>
                    </thead>
                    <tbody>
                        {movies.map((movie) => (
                            <tr key={movie.id} className="border-b">
                                <td className="px-4 py-2">{movie.name}</td>
                                <td className="px-4 py-2">{new Date(movie.releaseDate).toLocaleDateString()}</td>
                                <td className="px-4 py-2">
                                    <Link to={`/movies/edit/${movie.id}`} className="text-blue-500 hover:underline mr-4">Editar</Link>
                                    <button onClick={() => handleDelete(movie.id)} className="text-red-500 hover:underline">Eliminar</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default ManageMoviesPage;