import { FiChevronLeft, FiFilm, FiHome, FiLogOut, FiMessageSquare, FiStar, FiUsers } from "react-icons/fi";
import { NavLink } from "react-router-dom";

const AdminSidebar = () => {
  return (
    <aside className="w-64 h-screen bg-[#101828] text-white flex flex-col">
      <div className="px-6 py-5 border-b border-gray-700">
        <h1 className="text-2xl font-bold tracking-wide">
          Movie<span className="text-blue-500">Stream</span>
        </h1>
      </div>

      <div className="px-6 py-4 border-b border-gray-700">
        <div className="flex items-center gap-3">
          <div className="w-10 h-10 rounded-full bg-white/10 flex items-center justify-center">
            <FiUsers size={20} />
          </div>
          <div>
            <p className="text-xs text-blue-500">Admin</p>
            <p className="font-medium text-sm">John Doe</p>
          </div>
          <button className="ml-auto text-gray-400 hover:text-white">
            <FiLogOut />
          </button>
        </div>
      </div>

      <nav className="flex-1 px-4 py-6 space-y-2 text-sm">
        <SidebarItem icon={<FiHome />} label="Dashboard" to="/admin" />
        <SidebarItem icon={<FiFilm />} label="Movies" to="/admin/manage-movies" />
        <SidebarItem icon={<FiUsers />} label="Genres" to="/admin/manage-genres" />
        <SidebarItem icon={<FiMessageSquare />} label="Companies" to="/admin/manage-production-companies" />
        <SidebarItem icon={<FiChevronLeft />} label="Back to Home" to="/" />
      </nav>
    </aside>
  );
};

const SidebarItem = ({ icon, label, to }: { icon: React.ReactNode; label: string, to: string }) => (
    <NavLink
        to={to}
        end
        className={({ isActive }) =>
            `w-full flex items-center gap-3 px-2 py-2 rounded transition ${
                isActive ? "bg-blue-500/20 text-blue-300" : "hover:bg-white/10"
            }`
        }
        >
        {icon}
        {label}
    </NavLink>
);

export default AdminSidebar;