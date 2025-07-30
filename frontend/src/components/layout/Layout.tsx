import { Outlet } from "react-router-dom"
import { NavBar } from "./NavBar"
import { Footer } from "./Footer"

export const Layout = () => {
  return (
    <div className="bg-[#020916] min-h-screen text-white flex flex-col">
        <NavBar />
        <main className="pt-20 flex-grow">
            <Outlet />
        </main>
        <Footer />
    </div>
  )
}
