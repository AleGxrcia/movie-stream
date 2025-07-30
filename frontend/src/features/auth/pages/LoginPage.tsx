import { LoginForm } from "../components/LoginForm"

const LoginPage = () => {
	return (
		<div className="bg-gray-900 min-h-screen flex items-center justify-center p-4">
			<div className="w-full max-w-md bg-gray-800 rounded-lg shadow-lg p-8">
				<h2 className="text-3xl font-bold text-center text-white mb-6">
					Login to your account
				</h2>
				<LoginForm />
			</div>
		</div>
	);
};

export default LoginPage;