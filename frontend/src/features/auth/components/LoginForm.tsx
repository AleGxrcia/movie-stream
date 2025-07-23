import { useDispatch } from "react-redux";
import z from "zod";
import type { AppDispatch } from "../../../app/store";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { login } from "../slices/authSlice";

const schema = z.object({
	email: z.email({ message: 'Invalid email address' }),
	password: z.string().min(6, { message: 'Password must be at least 6 characters' }),
});

type LoginFormInputs = z.infer<typeof schema>;

export const LoginForm = () => {
	const dispatch = useDispatch<AppDispatch>();
	const { register, handleSubmit, formState: { errors } } = useForm<LoginFormInputs>({
		resolver: zodResolver(schema),
	});

	const onSubmit = (data: LoginFormInputs) => {
		dispatch(login(data));
	};

	return (
		<form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
			<div>
				<label
					htmlFor="email"
					className="block text-sm font-medium text-gray-700"
				>
					Email
				</label>
				<input
					id="email"
					type="text"
					{...register('email')}
					className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
				/>
				{errors.email && <p className="text-red-500 text-xs mt-1">{errors.email.message}</p>}
			</div>
			<div>
				<label
					htmlFor="password"
					className="block text-sm font-medium text-gray-700"
				>
					Password
				</label>
				<input
					id="password"
					type="password"
					{...register('password')}
          className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"	
				/>
				{errors.password && <p className="text-red-500 text-xs mt-1">{errors.password.message}</p>}
			</div>
			<button
				type="submit"
				className="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
			>
				Login
			</button>
		</form>
	);
};
