import { useDispatch } from "react-redux";
import z from "zod";
import type { AppDispatch } from "../../../app/store";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { register as registerAction } from '../slices/authSlice';

const schema = z.object({
  firstName: z.string().min(1, { message: 'First name is required' }).max(50, { message: 'First name must be at most 50 characters' }),
  lastName: z.string().min(1, { message: 'Last name is required' }).max(50, { message: 'Last name must be at most 50 characters' }),
	userName: z.string().min(3, { message: 'Username must be at least 3 characters' }),
	email: z.string().email({ message: 'Invalid email address' }),
	password: z.string().min(6, { message: 'Password must be at least 6 characters' }),
	confirmPassword: z.string(),
  phoneNumber: z.string().min(7, { message: 'Phone number is too short' }).max(15, { message: 'Phone number is too long' }),
}).refine(data => data.password === data.confirmPassword, {
	message: "Passwords don't match",
	path: ['confirmPassword'],
});

type RegisterFormInputs = z.infer<typeof schema>;

export const RegisterForm = () => {
	const dispatch = useDispatch<AppDispatch>();
	const { register, handleSubmit, formState: { errors } } = useForm<RegisterFormInputs>({
		resolver: zodResolver(schema),
	});

	const onSubmit = (data: RegisterFormInputs) => {
		dispatch(registerAction(data));
	};

  const inputClasses = "mt-1 block w-full px-3 py-2 bg-gray-800 border border-gray-600 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm text-white";

	return(
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      <div>
        <label htmlFor="firstName" className="block text-sm font-medium text-gray-300">
          First Name
        </label>
        <input
          id="firstName"
          type="text"
          {...register('firstName')}
          className={inputClasses}
        />
        {errors.firstName && <p className="text-red-500 text-xs mt-1">{errors.firstName.message}</p>}
      </div>

      <div>
        <label htmlFor="lastName" className="block text-sm font-medium text-gray-300">
          Last Name
        </label>
        <input
          id="lastName"
          type="text"
          {...register('lastName')}
          className={inputClasses}
        />
        {errors.lastName && <p className="text-red-500 text-xs mt-1">{errors.lastName.message}</p>}
      </div>

      <div>
        <label
          htmlFor="userName"
          className="block text-sm font-medium text-gray-300"
        >
          Username
        </label>
        <input
          id="userName"
          type="text"
          {...register('userName')}
          className={inputClasses}
        />
        {errors.userName && <p className="text-red-500 text-xs mt-1">{errors.userName.message}</p>}
      </div>
      <div>
        <label htmlFor="phoneNumber" className="block text-sm font-medium text-gray-300">
          Phone Number
        </label>
        <input
          id="phoneNumber"
          type="tel"
          {...register('phoneNumber')}
          className={inputClasses}
        />
        {errors.phoneNumber && <p className="text-red-500 text-xs mt-1">{errors.phoneNumber.message}</p>}
      </div>
      <div>
        <label
          htmlFor="email"
          className="block text-sm font-medium text-gray-300"
        >
          Email
        </label>
        <input
          id="email"
          type="email"
          {...register('email')}
          className={inputClasses}
        />
        {errors.email && <p className="text-red-500 text-xs mt-1">{errors.email.message}</p>}
      </div>
      <div>
        <label
          htmlFor="password"
          className="block text-sm font-medium text-gray-300"
        >
          Password
        </label>
        <input
          id="password"
          type="password"
          {...register('password')}
          className={inputClasses}
        />
        {errors.password && <p className="text-red-500 text-xs mt-1">{errors.password.message}</p>}
      </div>
      <div>
        <label
          htmlFor="confirmPassword"
          className="block text-sm font-medium text-gray-300"
        >
          Confirm Password
        </label>
        <input
          id="confirmPassword"
          type="password"
          {...register('confirmPassword')}
          className={inputClasses}
        />
        {errors.confirmPassword && <p className="text-red-500 text-xs mt-1">{errors.confirmPassword.message}</p>}
      </div>
      <button
        type="submit"
        className="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
      >
        Register
      </button>
		</form>
	);
};