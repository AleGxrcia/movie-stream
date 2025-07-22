import { LoginForm } from "../components/LoginForm"

const LoginPage = () => {
	return (
		<div className="flex justify-center items-center h-full">
			<div className="w-full max-w-md p-8 space-y-8 bg-white rounded-lg shadow-md">
				<h2 className="text-2xl font-bold text-center text-gray-900">
						Login to your account
				</h2>
				<LoginForm />
			</div>
		</div>
	);
};

export default LoginPage;