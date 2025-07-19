import { BiSolidCameraMovie } from 'react-icons/bi';
import { Link } from 'react-router-dom';

export const NavBar = () => {
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
      <div>Login</div>
    </header>
  )
}
