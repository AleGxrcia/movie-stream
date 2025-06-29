import { BiSolidCameraMovie } from 'react-icons/bi';

export const NavBar = () => {
  return (
    <header className="sticky top-0 z-50 py-4 flex p-6 justify-between bg-white items-center text-gray-800 shadow-md">
      <div className="flex items-center gap-2 text-3xl font-semibold">
        <BiSolidCameraMovie size={34} />
        <div>MovieStream</div>
      </div>
      <div>Search</div>
      <div>Login</div>
    </header>
  )
}
