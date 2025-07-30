import { useEffect, useState } from "react";
import { useAppDispatch, useAppSelector } from "../app/hooks";
import { deleteMovieAsync, fetchMoviesAsync, selectMovies, selectMoviesError, selectMoviesStatus } from "../features/movies/slices/moviesSlice";
import { Link } from "react-router-dom";
import { FiSearch, FiChevronDown, FiEdit2, FiTrash2, FiEye } from 'react-icons/fi';

const ManageMoviesPage = () => {
    const dispatch = useAppDispatch();
    const movies = useAppSelector(selectMovies);
    const status = useAppSelector(selectMoviesStatus);
    const error = useAppSelector(selectMoviesError);

    const [sortBy, setSortBy] = useState('Date created');
    const [searchQuery, setSearchQuery] = useState('');

    useEffect(() => {
        dispatch(fetchMoviesAsync());
    }, [dispatch, searchQuery, sortBy]);

    const handleDelete = (id: number) => {
        if (window.confirm('¿Estás seguro de que deseas eliminar esta película?')) {
            dispatch(deleteMovieAsync(id));
        }
    };

    if (status === 'loading') {
        return <div className="text-center text-white mt-24">Cargando...</div>;
    }

    if (status === 'failed') {
        return <div className="text-center text-red-500 mt-24">Error: {error}</div>;
    }

    return (
        <div className="p-6 bg-gray-900 min-h-screen">
            <div className="flex justify-between items-center mb-6">
                <div>
                    <h1 className="text-2xl font-bold text-white">Catalog</h1>
                    <p className="text-gray-400">{movies.length} total</p>
                </div>
                
                <div className="flex items-center space-x-4">
                    <div className="flex items-center">
                        <span className="text-gray-400 mr-2">Sort by:</span>
                        <div className="relative">
                            <button className="flex items-center text-white font-medium">
                                {sortBy} <FiChevronDown className="ml-1" />
                            </button>
                        </div>
                    </div>
                    
                    <div className="relative">
                        <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                            <FiSearch className="text-gray-400" />
                        </div>
                        <input
                        type="text"
                        placeholder="Find movie..."
                        className="pl-10 pr-4 py-2 bg-gray-800 border border-gray-700 rounded-md text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                        value={searchQuery}
                        onChange={(e) => setSearchQuery(e.target.value)}
                        />
                    </div>
                    <Link to="/movies/create" className="bg-blue-600 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-lg transition-colors">
                        Create Movie
                    </Link>
                </div>
            </div>

            <div className="bg-gray-800 rounded-lg shadow overflow-hidden">
                <div className="overflow-x-auto">
                    <table className="min-w-full divide-y divide-gray-700">
                        <thead className="bg-gray-700">
                            <tr>
                                <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">ID</th>
                                <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">TITLE</th>
                                <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">DESCRIPTION</th>
                                <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">GENRES</th>
                                <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">PROD. COMPANY</th>
                                <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">RELEASE DATE</th>
                                <th scope="col" className="px-6 py-3 text-center text-xs font-medium text-gray-300 uppercase tracking-wider">ACTIONS</th>
                            </tr>
                        </thead>
                        <tbody className="bg-gray-800 divide-y divide-gray-700">
                            {movies.map((movie) => (
                                <tr key={movie.id} className="hover:bg-gray-700">
                                    <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-white">{movie.id}</td>
                                    <td className="px-6 py-4 whitespace-nowrap text-sm text-white">{movie.name}</td>
                                    <td className="px-6 py-4 whitespace-nowrap text-sm text-white max-w-xs truncate" title={movie.description}>
                                        {movie.description}
                                    </td>
                                    <td className="px-6 py-4 whitespace-nowrap text-sm text-white">
                                        {movie.genres?.map(g => g.name).join(', ')}
                                    </td>
                                    <td className="px-6 py-4 whitespace-nowrap text-sm text-white">{movie.productionCompany?.name}</td>
                                    <td className="px-6 py-4 whitespace-nowrap text-sm text-white">{new Date(movie.releaseDate).toLocaleDateString()}</td>
                                    <td className="px-6 py-4 whitespace-nowrap text-sm text-center">
                                        <div className="flex items-center justify-center space-x-4">
                                            <Link to={`/movies/${movie.id}`} className="text-blue-400 hover:text-blue-300">
                                                <FiEye size={18} />
                                            </Link>
                                            <Link to={`/movies/edit/${movie.id}`} className="text-green-400 hover:text-green-300">
                                                <FiEdit2 size={18} />
                                            </Link>
                                            <button onClick={() => handleDelete(movie.id)} className="text-red-400 hover:text-red-300">
                                                <FiTrash2 size={18} />
                                            </button>
                                        </div>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
};

export default ManageMoviesPage;