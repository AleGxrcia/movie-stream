import { createBrowserRouter } from "react-router-dom";
import { Layout } from "../components/layout/Layout";
import HomePage from "../pages/HomePage";
import NotFoundPage from "../pages/NotFoundPage";
import MoviesListPage from "../pages/MoviesListPage";
import RegisterPage from "../features/auth/pages/RegisterPage";
import LoginPage from "../features/auth/pages/LoginPage";
import { PrivateRoute } from "./PrivateRoute";
import UnauthorizedPage from "../pages/UnathorizedPage";
import ManageMoviesPage from "../pages/ManageMoviesPage";
import CreateMoviePage from "../features/movies/pages/CreateMoviePage";
import MovieDetailsPage from "../features/movies/pages/MovieDetailsPage";
import EditMoviePage from "../features/movies/pages/EditMoviePage";
import ManageGenresPage from "../features/genres/pages/ManageGenresPage";
import ManageProductionCompaniesPage from "../features/productionCompanies/pages/ManageProductionCompaniesPage";
import SaveGenrePage from "../features/genres/pages/SaveGenrePage";
import SaveProductionCompanyPage from "../features/productionCompanies/pages/SaveProductionCompanyPage";

const router = createBrowserRouter([
    {
        path: '/',
        element: <Layout />,
        errorElement: <NotFoundPage />,
        children: [
            { index: true, element: <HomePage /> },
            { path: 'login', element: <LoginPage /> },
            { path: 'register', element: <RegisterPage /> },
            { path: 'unauthorized', element: <UnauthorizedPage /> },
            { path: 'movies', element: <MoviesListPage /> },
            { path: 'movies/:id', element: <MovieDetailsPage /> },
            { path: 'admin/manage-movies', element: <ManageMoviesPage /> },
            { path: 'admin/movies/create', element: <CreateMoviePage /> },
            { path: 'admin/movies/edit/:id', element: <EditMoviePage /> },
            { path: 'admin/manage-genres', element: <ManageGenresPage /> },
            { path: 'admin/genres/create', element: <SaveGenrePage /> },
            { path: 'admin/genres/edit/:id', element: <SaveGenrePage /> },
            { path: 'admin/manage-production-companies', element: <ManageProductionCompaniesPage /> },
            { path: 'admin/production-companies/create', element: <SaveProductionCompanyPage /> },
            { path: 'admin/production-companies/edit/:id', element: <SaveProductionCompanyPage /> },
            {
                element: <PrivateRoute allowedRoles={['User']} />,
                children: [
                    // { path: 'movies', element: <MoviesListPage /> },
                    // { path: 'movies/:id', element: <MovieDetailsPage /> },
                ]
            },
            {
                element: <PrivateRoute allowedRoles={['Admin', 'ContentManager']} />,
                children: [
                    // { path: 'manage-movies', element: <ManageMoviesPage /> },
                    // { path: 'movies/create', element: <CreateMoviePage /> },
                    // { path: 'movies/edit/:id', element: <EditMoviePage /> },
                ]
            }
        
        ],
    },
]);

export default router;