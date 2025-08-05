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
import AdminLayout from "../components/layout/admin/AdminLayout";
import AdminDashboardPage from "../pages/admin/AdminDashboardPage";

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
        ],
    },
    {
        path: '/admin',
        element: (
            <PrivateRoute allowedRoles={['Admin', 'ContentManager']}>
                <AdminLayout />
            </PrivateRoute>
        ),
        errorElement: <NotFoundPage />,
        children: [
            { index: true, element: <AdminDashboardPage /> },
            { path: 'manage-movies', element: <ManageMoviesPage /> },
            { path: 'movies/create', element: <CreateMoviePage /> },
            { path: 'movies/edit/:id', element: <EditMoviePage /> },
            { path: 'manage-genres', element: <ManageGenresPage /> },
            { path: 'genres/create', element: <SaveGenrePage /> },
            { path: 'genres/edit/:id', element: <SaveGenrePage /> },
            { path: 'manage-production-companies', element: <ManageProductionCompaniesPage /> },
            { path: 'production-companies/create', element: <SaveProductionCompanyPage /> },
            { path: 'production-companies/edit/:id', element: <SaveProductionCompanyPage /> },
        ]
    }
]);

export default router;