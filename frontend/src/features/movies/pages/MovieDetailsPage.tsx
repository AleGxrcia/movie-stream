import { useParams } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../../app/hooks";
import { fetchMovieByIdAsync, selectMoviesError, selectMoviesStatus, selectSelectedMovie } from "../slices/moviesSlice";
import { useEffect } from "react";

const MovieDetailsPage = () => {
    const { id } = useParams<{ id: string }>();
    const dispatch = useAppDispatch();
    const movie = useAppSelector(selectSelectedMovie);
    const status = useAppSelector(selectMoviesStatus);
    const error = useAppSelector(selectMoviesError);

    useEffect(() => {
        if (id) {
            dispatch(fetchMovieByIdAsync(Number(id)));
        }
    }, [dispatch, id]);

    if (status === 'loading') {
        return <div className="text-center">Cargando...</div>;
    }

    if (status === 'failed') {
        return <div className="text-center text-red-500">Error: {error}</div>;
    }

    if (!movie) {
        return <div className="text-center">No se encontró la película.</div>;
    }

    const { name, imageUrl, description, releaseDate, runtime, genres, productionCompany } = movie;

    const formatData = (dateString: string | null) => {
        if (!dateString) return 'Fecha desconocida';
        const date = new Date(dateString);
        return date.toLocaleDateString('es-ES', {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
        });
    };

    return (
        <div className="container mx-auto px-4 py-8">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
                <div>
                    <img src={imageUrl} alt={name} className="rounded-lg shadow-lg" />
                </div>
                <div>
                    <h1 className="text-4xl font-bold mb-4">{name}</h1>
                    <p className="text-gray-600 mb-4">{description}</p>
                    <div className="flex items-center mb-4">
                        <span className="font-bold mr-2">Fecha de lanzamiento:</span>
                        <span>{formatData(releaseDate)}</span>
                    </div>
                    <div className="flex items-center mb-4">
                        <span className="font-bold mr-2">Duración:</span>
                        <span>{runtime}</span>
                    </div>
                    <div className="flex items-center mb-4">
                        <span className="font-bold mr-2">Productora:</span>
                        <span>{productionCompany?.name}</span>
                    </div>
                    <div className="flex items-center">
                        <span className="font-bold mr-2">Géneros:</span>
                        <span>{genres?.map(g => g.name).join(', ')}</span>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default MovieDetailsPage;