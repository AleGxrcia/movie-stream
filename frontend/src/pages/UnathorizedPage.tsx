import { Link } from "react-router-dom";

const UnauthorizedPage = () => {
    return (
        <div className="flex flex-col items-center justify-center h-screen bg-gray-100">
            <h1 className="text-4xl font-bold text-red-600">403 - Unauthorized</h1>
            <p className="text-lg text-gray-700 mt-4">You do not have permission to access this page.</p>
            <Link to="/" className="mt-8 px-4 py-2 text-white bg-blue-500 rounded hover:bg-blue-600">
                Go to Homepage
            </Link>
        </div>
    );
};

export default UnauthorizedPage;