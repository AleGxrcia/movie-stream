import { useEffect, useState } from "react";
import { useAppDispatch, useAppSelector } from "../../../app/hooks";
import { deleteGenreAsync, fetchGenresAsync, selectGenres, selectGenresError, selectGenresStatus } from "../slices/genreSlice";
import { FiChevronDown, FiEdit2, FiSearch, FiTrash2 } from "react-icons/fi";
import { Link } from "react-router-dom";

const ManageGenresPage = () => {
    const dispatch = useAppDispatch();
    const genres = useAppSelector(selectGenres);
    const status = useAppSelector(selectGenresStatus);
    const error = useAppSelector(selectGenresError);

    const [sortBy] = useState('Name');
    const [searchQuery, setSearchQuery] = useState('');
    const [currentPage, setCurrentPage] = useState(1);

    useEffect(() => {
        dispatch(fetchGenresAsync());
    }, [dispatch, searchQuery, sortBy, currentPage]);

    const handleDelete = (id: number) => {
        if (window.confirm('¿Estás seguro de que deseas eliminar este genero?')) {
            dispatch(deleteGenreAsync(id));
        }
    };

    const handlePageChange = (newPage: number) => {
        setCurrentPage(newPage);
    };

    if (status === 'loading') {
        return <div className="text-center text-white mt-24">Cargando...</div>;
    }

    if (status === 'failed') {
        return <div className="text-center text-red-500 mt-24">Error: {error}</div>;
    }

    return (
        <div className="p-6 min-h-screen text-white">
            <div className="flex justify-between items-center mb-6">
                <div>
                    <h1 className="text-2xl font-bold">Manage Genres</h1>
                    <p className="text-gray-400">Total genres: {genres.length}</p>
                </div>
                <div className="flex items-center space-x-4">
                    <div className="flex items-center">
                        <span className="text-gray-400 mr-2">Sort by:</span>
                        <div className="relative">
                            <button className="flex items-center font-medium">
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
                            placeholder="Find genre..."
                            className="pl-10 pr-4 py-2 bg-gray-800 border border-gray-700 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                            value={searchQuery}
                            onChange={(e) => setSearchQuery(e.target.value)}
                        />
                    </div>
                    <Link to="/admin/genres/create" className="bg-blue-600 hover:bg-blue-700 font-bold py-2 px-4 rounded-lg transition-colors">
                        Create Genre
                    </Link>
                </div>
            </div>

            <div className="bg-gray-800 rounded-lg shadow overflow-hidden mb-[2%]">
                <div className="overflow-x-auto">
                    <table className="min-w-full divide-y divide-gray-700">
                        <thead className="bg-gray-700">
                            <tr>
                                <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">ID</th>
                                <th scope="col" className="px-6 py-3 text-left text-xs font-medium text-gray-300 uppercase tracking-wider">Name</th>
                                <th scope="col" className="px-6 py-3 text-center text-xs font-medium text-gray-300 uppercase tracking-wider">Actions</th>
                            </tr>
                        </thead>
                        <tbody className="bg-gray-800 divide-y divide-gray-700">
                            {genres.map((genre) => (
                                <tr key={genre.id} className="hover:bg-gray-700">
                                    <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">{genre.id}</td>
                                    <td className="px-6 py-4 whitespace-nowrap text-sm">{genre.name}</td>
                                    <td className="px-6 py-4 whitespace-nowrap text-sm text-center">
                                        <div className="flex items-center justify-center space-x-4">
                                            <Link to={`/admin/genres/edit/${genre.id}`} className="text-green-400 hover:text-green-300">
                                                <FiEdit2 size={18} />
                                            </Link>
                                            <button onClick={() => handleDelete(genre.id)} className="text-red-400 hover:text-red-300">
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

export default ManageGenresPage;