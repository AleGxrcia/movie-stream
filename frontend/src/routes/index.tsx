import { createBrowserRouter } from "react-router-dom";
import { Layout } from "../components/layout/Layout";
import HomePage from "../pages/HomePage";
import NotFoundPage from "../pages/NotFoundPage";
import MoviesListPage from "../pages/MoviesListPage";
import RegisterPage from "../features/auth/pages/RegisterPage";
import LoginPage from "../features/auth/pages/LoginPage";

const router = createBrowserRouter([
    {
        path: '/',
        element: <Layout />,
        errorElement: <NotFoundPage />,
        children: [
            { index: true, element: <HomePage /> },
            { path: 'movies', element: <MoviesListPage /> },
            { path: 'login', element: <LoginPage /> },
            { path: 'register', element: <RegisterPage /> },
        ],
    },
]);

export default router;