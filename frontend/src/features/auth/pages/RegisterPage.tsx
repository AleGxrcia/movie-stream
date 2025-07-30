import { RegisterForm } from "../components/RegisterForm"

const RegisterPage = () => {
	return (
		<div className="bg-gray-900 min-h-screen flex items-center justify-center p-4">
			<div className="w-full max-w-md bg-gray-800 rounded-lg shadow-lg p-8">
				<h2 className="text-3xl font-bold text-center text-white mb-6">
					Create an account
				</h2>
				<RegisterForm />
			</div>
		</div>
	);
};

export default RegisterPage;