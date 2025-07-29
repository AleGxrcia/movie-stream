import { useNavigate } from "react-router-dom";
import { useAppDispatch } from "../../../app/hooks";
import { createMovieAsync } from "../slices/moviesSlice";
import MovieForm from "../components/MovieForm";


const CreateMoviePage = () => {
    const dispatch = useAppDispatch();
    const navigate = useNavigate();

    const handleSubmit = (data: FormData) => {
        dispatch(createMovieAsync(data)).then(() => {
            navigate('/manage-movies');
        });
    };

    return (
        <div className="container mx-auto px-4 py-8">
            <h1 className="text-3xl font-bold mb-8">Crear Película</h1>
            <MovieForm onSubmit={handleSubmit} />
        </div>
    );
};

export default CreateMoviePage;
