import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';

const formSchema = z.object({
  name: z.string().min(1, 'El nombre es requerido'),
});

type FormValues = z.infer<typeof formSchema>;

interface SimpleFormProps {
  onSubmit: (data: FormValues) => void;
  defaultValues?: FormValues;
  title: string;
  placeholder: string;
}

const SimpleForm = ({ onSubmit, defaultValues, title, placeholder }: SimpleFormProps) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: defaultValues || { name: '' },
  });

  const inputClasses =
    'mt-1 block w-full px-3 py-2 bg-gray-800 border border-gray-600 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm text-white';

  return (
    <div className="max-w-xl mx-auto p-6 bg-gray-900 rounded-lg shadow-md">
      <h1 className="text-2xl font-bold mb-6 text-white">{title}</h1>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div className="space-y-4">
          <input
            type="text"
            placeholder={placeholder}
            className={inputClasses}
            {...register('name')}
          />
          {errors.name && <p className="text-red-500 text-xs">{errors.name.message}</p>}
        </div>

        <button
          type="submit"
          className="mt-6 w-full bg-blue-600 text-white py-3 px-4 rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
        >
          Guardar
        </button>
      </form>
    </div>
  );
};

export default SimpleForm;