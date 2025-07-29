import { useNavigate, useParams } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../../app/hooks";
import { fetchMovieByIdAsync, selectSelectedMovie, updateMovieAsync } from "../slices/moviesSlice";
import { useEffect } from "react";
import MovieForm from "../components/MovieForm";

const EditMoviePage = () => {
    const { id } = useParams<{ id: string }>();
    const dispatch = useAppDispatch();
    const navigate = useNavigate();
    const movie = useAppSelector(selectSelectedMovie);

    useEffect(() => {
        if (id) {
            dispatch(fetchMovieByIdAsync(Number(id)));
        }
    }, [dispatch, id]);

    const handleSubmit = (data: FormData) => {
        if (id) {
            console.log('dasdasd');
            dispatch(updateMovieAsync({ id: Number(id), movieData: data })).then(() => {
                navigate('/manage-movies');
            });
        }
    };

    return (
        <div className="container mx-auto px-4 py-8">
            <h1 className="text-3xl font-bold mb-8">Editar Película</h1>
            {movie && <MovieForm onSubmit={handleSubmit} defaultValues={movie} />}
        </div>
    );
};

export default EditMoviePage;