import type { Movie } from '../types/movie.types';

interface MovieDetailsProps {
    movie: Movie;
}

const MovieDetails = ({ movie }: MovieDetailsProps) => {
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
            <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
                <div className="md:col-span-1">
                    <img src={imageUrl} alt={name} className="rounded-lg shadow-lg" />
                </div>
                <div className="md:col-span-2">
                    <h1 className="text-4xl font-bold mb-4">{name}</h1>
                    <p className="text-lg text-gray-700 mb-4">{description}</p>
                    <div className="flex items-center mb-4">
                        <span className="font-bold mr-2">Estreno:</span>
                        <span>{formatData(releaseDate)}</span>
                    </div>
                    <div className="flex items-center mb-4">
                        <span className="font-bold mr-2">Duración:</span>
                        <span>{runtime}</span>
                    </div>
                    <div className="flex items-center mb-4">
                        <span className="font-bold mr-2">Productora:</span>
                        <span>{productionCompany.name}</span>
                    </div>
                    <div className="flex items-center">
                        <span className="font-bold mr-2">Géneros:</span>
                        <div className="flex flex-wrap">
                            {genres?.map((genre) => (
                                <span key={genre.id} className="bg-gray-200 text-gray-800 px-2 py-1 rounded-full text-sm mr-2 mb-2">
                                    {genre.name}
                                </span>
                            ))}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default MovieDetails;