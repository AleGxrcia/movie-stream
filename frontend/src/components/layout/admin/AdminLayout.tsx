import { Outlet } from "react-router-dom";
import AdminSidebar from "./AdminSidebar";

const AdminLayout = () => {
    return (
        <div className="flex">
            <AdminSidebar />
            <main className="flex-grow p-6">
                <Outlet />
            </main>
        </div>
    );
};

export default AdminLayout;