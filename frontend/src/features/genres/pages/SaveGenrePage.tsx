import { useNavigate, useParams } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../../app/hooks";
import { clearSelectedGenre, createGenreAsync, fetchGenreByIdAsync, selectGenresStatus, selectSelectedGenre, updateGenreAsync } from "../slices/genreSlice";
import { useEffect } from "react";
import SimpleForm from "../../../components/common/SimpleForm";

const SaveGenrePage = () => {
    const dispatch = useAppDispatch();
    const navigate = useNavigate();
    const { id } = useParams<{ id: string }>();
    const selectedGenre = useAppSelector(selectSelectedGenre);
    const status = useAppSelector(selectGenresStatus);

    useEffect(() => {
        dispatch(clearSelectedGenre());
        if (id) {
            dispatch(fetchGenreByIdAsync(Number(id)));
        }

        return () => {
            dispatch(clearSelectedGenre());
        };
    }, [dispatch, id]);

    const handleSubmit = (data: { name: string }) => {
        if (id) {
            dispatch(updateGenreAsync({ id: Number(id), name: data.name })).then(() => {
                navigate('/manage-genres');
            });
        } else {
            dispatch(createGenreAsync(data)).then(() => {
                navigate('/manage-genres');
            });
        }
    };

    if (status === 'loading') {
        return (
            <div className="container mx-auto px-4 py-8">
                <div className="text-center text-white">Loading...</div>
            </div>
        );
    }

    const title = id ? "Edit Genre" : "Create Genre";
    const placeholder = "Name";

    return (
        <div className="container mx-auto px-4 py-8 text-white">
            <SimpleForm 
                onSubmit={handleSubmit} 
                defaultValues={selectedGenre || { name: '' }}
                title={title}
                placeholder={placeholder}
            />
        </div>
    );
};

export default SaveGenrePage;