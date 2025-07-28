import { useSelector } from "react-redux";
import type { RootState } from "../app/store";
import { Navigate, Outlet, useLocation } from "react-router-dom";

interface PrivateRouteProps {
    allowedRoles?: string[];
}

export const PrivateRoute = ({ allowedRoles }: PrivateRouteProps) => {
    const { isAuthenticated, user } = useSelector((state: RootState) => state.auth);
    const location = useLocation();

    if (!isAuthenticated) {
        return <Navigate to="/login" state={{ from: location }} replace />;
    }

    const userRoles = user?.roles || [];
    const hasRequiredRole = allowedRoles ? allowedRoles.some(role => userRoles.includes(role)) : true;

    if (!hasRequiredRole) {
        return <Navigate to="/unauthorized" replace />;
    }

    return <Outlet />;
}