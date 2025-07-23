import { RegisterForm } from "../components/RegisterForm"

const RegisterPage = () => {
	return (
		<div className="flex justify-center items-center h-full">
			<div className="w-full max-w-md p-8 space-y-8 bg-white rounded-lg shadow-md">
				<h2 className="text-2xl font-bold text-center text-gray-900">
					Create an account
				</h2>
				<RegisterForm />
			</div>
		</div>
	);
};

export default RegisterPage;