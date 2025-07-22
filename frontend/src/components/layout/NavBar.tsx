import { BiSolidCameraMovie } from 'react-icons/bi';
import { useDispatch, useSelector } from 'react-redux';
import { Link } from 'react-router-dom';
import type { AppDispatch, RootState } from '../../app/store';
import { logout } from '../../features/auth/slices/authSlice';

export const NavBar = () => {
  const { isAuthenticated } = useSelector((state: RootState) => state.auth);
  const dispatch = useDispatch<AppDispatch>();

  const handleLogout = () => {
    dispatch(logout());
  }

  return (
    <header className="sticky top-0 z-50 py-4 flex p-6 justify-between bg-white items-center text-gray-800 shadow-md">
      <div className="flex items-center gap-2 text-3xl font-semibold">
        <BiSolidCameraMovie size={34} />
        <Link to="/">MovieStream</Link>
      </div>
      <nav>
        <ul className="flex gap-4">
          <li>
            <Link to="/movies">Películas</Link>
          </li>
        </ul>
      </nav>
      <div>Search</div>
      <div className='flex gap-4'>
        {isAuthenticated ? (
          <>
            <Link to="/profile">Profile</Link>
            <button onClick={handleLogout}>Logout</button>
          </>
        ) : (
          <>
            <Link to="/login">Profile</Link>
            <Link to="/register">Profile</Link>
          </>
        )}
      </div>
    </header>
  );
};
