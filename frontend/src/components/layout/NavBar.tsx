import { BiSolidCameraMovie } from 'react-icons/bi';
import { useDispatch, useSelector } from 'react-redux';
import { Link, useNavigate } from 'react-router-dom';
import type { AppDispatch, RootState } from '../../app/store';
import { logout } from '../../features/auth/slices/authSlice';

export const NavBar = () => {
  const { isAuthenticated, user } = useSelector((state: RootState) => state.auth);
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();

  const handleLogout = () => {
    dispatch(logout());
    navigate('/login');
  }

  return (
    <header className="bg-[#101828] shadow-lg fixed top-0 left-0 right-0 z-50">
      <div className="container mx-auto px-6 py-4">
        <div className="flex items-center justify-between">
          <div className="flex items-center">
            <Link to="/" className="flex items-center text-white text-2xl font-bold">
              <BiSolidCameraMovie size={34} className="text-[#3b82f6] mr-2" />
              <span>MovieStream</span>
            </Link>
          </div>

          <nav className="hidden md:flex items-center space-x-8">
            <Link to="/" className="text-gray-300 hover:text-white transition-colors">Home</Link>
            <Link to="/movies" className="text-gray-300 hover:text-white transition-colors">Movies</Link>
            {user?.roles.includes('Admin') && (
              <>
                {/* <Link to="/manage-movies" className="text-gray-300 hover:text-white transition-colors">Manage Movies</Link>
                <Link to="/manage-genres" className="text-gray-300 hover:text-white transition-colors">Manage Genres</Link>
                <Link to="/manage-production-companies" className="text-gray-300 hover:text-white transition-colors">Manage Prod. Companies</Link> */}
              </>            
            )}
          </nav>

          <div className="flex items-center space-x-4">
            {isAuthenticated ? (
              <>
                <span className="text-white font-semibold hidden sm:block">{user?.userName}</span>
                <button onClick={handleLogout} className="bg-red-600 hover:bg-red-700 text-white font-bold py-2 px-4 rounded-lg transition-colors">
                  Logout
                </button>
              </>
            ) : (
              <>
                <Link to="/login" className="text-gray-300 hover:text-white transition-colors">Login</Link>
                <Link to="/register" className="bg-[#3b82f6] hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-lg transition-colors">
                  Register
                </Link>
              </>
            )}
          </div>
        </div>
      </div>
    </header>
  );
};