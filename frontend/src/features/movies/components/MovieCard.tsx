import type { Movie } from "../types/movie.types"

interface MovieCardProps {
    movie: Movie;
}

const MovieCard = ({ movie }: MovieCardProps) => {
    const { name, imageUrl, releaseDate } = movie;

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
        <div className="max-w-sm rounded overflow-hidden shadow-lg">
            <img className="w-full" src={imageUrl} alt={name} />
            <div className="px-6 py-4">
                <div className="font-bold text-xl mb-2">{name}</div>
                <p className="text-gray-700 text-base">
                {formatData(releaseDate)}
                </p>
            </div>
        </div>
    );
}

export default MovieCard;