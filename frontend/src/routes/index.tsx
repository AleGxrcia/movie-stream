import { createBrowserRouter } from "react-router-dom";
import { Layout } from "../components/layout/Layout";
import HomePage from "../pages/HomePage";
import NotFoundPage from "../pages/NotFoundPage";
import MoviesListPage from "../pages/MoviesListPage";
import RegisterPage from "../features/auth/pages/RegisterPage";
import LoginPage from "../features/auth/pages/LoginPage";
import { PrivateRoute } from "./PrivateRoute";
import UnauthorizedPage from "../pages/UnathorizedPage";

const router = createBrowserRouter([
    {
        path: '/',
        element: <Layout />,
        errorElement: <NotFoundPage />,
        children: [
            { index: true, element: <HomePage /> },
            // { path: 'movies', element: <MoviesListPage /> },
            { path: 'login', element: <LoginPage /> },
            { path: 'register', element: <RegisterPage /> },
            { path: 'unauthorized', element: <UnauthorizedPage /> },
            {
                element: <PrivateRoute allowedRoles={['Admin', 'ContentManager', 'User']} />,
                children: [
                    { path: 'movies', element: <MoviesListPage /> },
                ]
            }
        
        ],
    },
]);

export default router;