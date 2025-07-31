import { useNavigate, useParams } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../../app/hooks";
import { clearSelectedProductionCompany, createProductionCompanyAsync,
         fetchProductionCompanyByIdAsync,
         selectProductionCompaniesStatus,
         selectSelectedProductionCompany,
         updateProductionCompanyAsync 
} from "../slices/productionCompaniesSlice";
import { useEffect } from "react";
import SimpleForm from "../../../components/common/SimpleForm";

const SaveProductionCompanyPage = () => {
    const dispatch = useAppDispatch();
    const navigate = useNavigate();
    const { id } = useParams<{ id: string }>();
    const selectedCompany = useAppSelector(selectSelectedProductionCompany);
    const status = useAppSelector(selectProductionCompaniesStatus);

    useEffect(() => {
        dispatch(clearSelectedProductionCompany());
        if (id) {
            dispatch(fetchProductionCompanyByIdAsync(Number(id)));
        }

        return () => {
            dispatch(clearSelectedProductionCompany());
        };
    }, [dispatch, id]);

    const handleSubmit = (data: { name: string }) => {
        if (id) {
            dispatch(updateProductionCompanyAsync({ id: Number(id), name: data.name })).then(() => {
                navigate('/manage-production-companies');
            });
        } else {
            dispatch(createProductionCompanyAsync(data)).then(() => {
                navigate('/manage-production-companies');
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

    const title = id ? "Edit Production Company" : "Create Production Company";
    const placeholder = "Name";

    return (
        <div className="container mx-auto px-4 py-8">
            <SimpleForm 
                onSubmit={handleSubmit} 
                defaultValues={selectedCompany || { name: '' }}
                title={title}
                placeholder={placeholder}
            />
        </div>
    );
};

export default SaveProductionCompanyPage;