import { Link } from "react-router-dom";
import type { Movie } from "../types/movie.types"

interface MovieCardProps {
    movie: Movie;
}

const MovieCard = ({ movie }: MovieCardProps) => {
    const { id, name, imageUrl, releaseDate, runtime } = movie;

    const formatData = (dateString: string | null) => {
      if (!dateString) return 'Fecha desconocida';
      const date = new Date(dateString);
      return date.getFullYear();
    };

    const formatRuntime = (time: string) => {
      const [hours, minutes, seconds] = time?.split(':').map(Number);
      if ([hours, minutes, seconds].some(isNaN)) {
        return 'Duracion desconocida';
      }
      return hours * 60 + minutes + Math.floor(seconds / 60);
    };

    return (
        <div className="w-full max-w-[220px] bg-gray-900 rounded-lg overflow-hidden shadow-lg flex flex-col m-2 transition-transform duration-300 ease-in-out hover:scale-105">
            <Link to={`/movies/${id}`} className="h-full flex flex-col">
                {/* Poster con aspect ratio */}
                <div className="relative w-full aspect-[2/3]">
                    {imageUrl ? (
                        <img
                            className="absolute top-0 left-0 w-full h-full object-cover"
                            src={imageUrl}
                            alt={name}
                        />
                    ) : (
                        <div className="absolute top-0 left-0 w-full h-full bg-gradient-to-br from-blue-900 to-purple-800 flex items-center justify-center">
                            <div className="text-center">
                                <div className="text-white text-2xl font-bold mb-2">Poster</div>
                            </div>
                        </div>
                    )}

                    {/* Etiqueta HD */}
                    <div className="absolute top-2 right-2 bg-red-600 text-white text-xs font-bold px-2 py-1 rounded">
                        HD
                    </div>
                </div>

                {/* Info */}
                <div className="p-3 flex flex-col justify-between flex-grow bg-gray-800">
                    {/* Title */}
                    <h3 className="text-white font-medium text-sm truncate">
                        {name}
                    </h3>

                    {/* Year and Duration */}
                    <div className="flex justify-between items-center text-gray-300 text-xs mt-2">
                        <div className="flex items-center space-x-1.5">
                            <span>{formatData(releaseDate)}</span>
                            <span className="w-1 h-1 bg-gray-300 rounded-full"></span>
                            <span>{`${formatRuntime(runtime)}m`}</span>
                        </div>
                        <span className="bg-transparent border border-gray-600 text-gray-400 text-[10px] font-bold px-1.5 py-0.5 rounded">
                            Movie
                        </span>
                    </div>
                </div>
            </Link>
        </div>
    );
}

export default MovieCard;