import z from "zod";
import type { Movie } from "../types/movie.types";
import { useAppDispatch } from "../../../app/hooks";
import { useSelector } from "react-redux";
import { fetchProductionCompaniesAsync, selectProductionCompanies } from "../../productionCompanies/slices/productionCompaniesSlice";
import { fetchGenresAsync, selectGenres } from "../../genres/slices/genreSlice";
import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import ReactSelect from 'react-select';

const movieSchema = z.object({
  name: z.string().min(1, 'El nombre es requerido'),
  description: z.string().min(1, 'La descripción es requerida'),
  runtime: z.string().min(1, 'La duración es requerida'),
  releaseDate: z.string().min(1, 'La fecha de lanzamiento es requerida'),
  productionCompanyId: z.string().min(1, 'La productora es requerida'),
  genreIds: z.array(z.string()).min(1, 'Debes seleccionar al menos un género'),
  imageFile: z.any().optional(),
});

type MovieFormValues = z.infer<typeof movieSchema>;

interface MovieFormProps {
  onSubmit: (data: FormData) => void;
  defaultValues?: Movie;
}

const MovieForm = ({ onSubmit, defaultValues }: MovieFormProps) => {
    const dispatch = useAppDispatch();
    const genres = useSelector(selectGenres);
    const productionCompanies = useSelector(selectProductionCompanies);
    const [preview, setPreview] = useState<string | null>(defaultValues?.imageUrl || null);


  const { register, handleSubmit, formState: { errors }, setValue, watch } = useForm<MovieFormValues>({
    resolver: zodResolver(movieSchema),
    defaultValues: {
        ...defaultValues,
        releaseDate: defaultValues ? new Date(defaultValues.releaseDate).toISOString().split('T')[0] : '',
        productionCompanyId: defaultValues?.productionCompany.id.toString(),
        genreIds: defaultValues?.genres?.map(g => g.id.toString()) || [],
    }
  });

  useEffect(() => {
    dispatch(fetchGenresAsync());
    dispatch(fetchProductionCompaniesAsync());
  }, [dispatch]);

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => {
        setPreview(reader.result as string);
      };
      reader.readAsDataURL(file);
      setValue('imageFile', file);
    } else {
      setPreview(defaultValues?.imageUrl || null);
    }
  };

  const handleFormSubmit = (data: MovieFormValues) => {
    const formData = new FormData();
    formData.append('name', data.name);
    formData.append('description', data.description);
    formData.append('runtime', data.runtime);
    formData.append('releaseDate', data.releaseDate);
    formData.append('productionCompanyId', data.productionCompanyId);
    data.genreIds.forEach(id => formData.append('genreIds', id));
    if (data.imageFile) {
      formData.append('imageFile', data.imageFile);
    }
    onSubmit(formData);
  };

  const onInvalid = (errors: any) => {
    console.error(errors);
  };
  
  const genreOptions = genres.map(g => ({ value: g.id.toString(), label: g.name }));
  const selectedGenres = watch('genreIds')?.map(id => genreOptions.find(g => g.value === id)).filter(Boolean);

  const customStyles = {
    control: (provided: any) => ({
      ...provided,
      backgroundColor: '#1f2937',
      borderColor: '#4b5563',
      color: 'white',
      '&:hover': {
        borderColor: '#6b7280',
      },
    }),
    menu: (provided: any) => ({
      ...provided,
      backgroundColor: '#1f2937',
    }),
    option: (provided: any, state: { isSelected: any; isFocused: any; }) => ({
      ...provided,
      backgroundColor: state.isSelected ? '#3b82f6' : state.isFocused ? '#374151' : '#1f2937',
      color: 'white',
    }),
    multiValue: (provided: any) => ({
      ...provided,
      backgroundColor: '#3b82f6',
    }),
    multiValueLabel: (provided: any) => ({
      ...provided,
      color: 'white',
    }),
    input: (provided: any) => ({
        ...provided,
        color: 'white',
    }),
    placeholder: (provided: any) => ({
        ...provided,
        color: '#9ca3af',
    }),
    singleValue: (provided: any) => ({
        ...provided,
        color: 'white',
    }),
  };

  const inputClasses = "mt-1 block w-full px-3 py-2 bg-gray-800 border border-gray-600 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm text-white";

  return (
    <div className="max-w-4xl mx-auto p-6 bg-gray-900 rounded-lg shadow-md">
      <h1 className="text-2xl font-bold mb-6 text-white">Add new item</h1>
      
      <form onSubmit={handleSubmit(handleFormSubmit, onInvalid)}>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
          <div className="md:col-span-1">
            <label className="block text-sm text-gray-400 mb-1">Upload cover</label>
            <div className="border-2 border-dashed border-gray-600 rounded w-full h-80 flex items-center justify-center relative">
              <input
                type="file"
                className="absolute inset-0 w-full h-full opacity-0 cursor-pointer"
                onChange={handleFileChange}
                accept="image/*"
              />
              {preview ? (
                <img 
                  src={preview} 
                  alt="Preview" 
                  className="w-full h-full object-cover rounded"
                />
              ) : (
                <span className="text-gray-500 text-3xl">+</span>
              )}
            </div>
            {errors.imageFile && <p className="text-red-500 text-xs mt-1">{errors.imageFile.message as string}</p>}
          </div>

          <div className="md:col-span-2 space-y-4">
            <input
              type="text"
              placeholder="Title"
              className={inputClasses}
              {...register('name')}
            />
            {errors.name && <p className="text-red-500 text-xs">{errors.name.message}</p>}
            
            <textarea
              placeholder="Description"
              rows={4}
              className={inputClasses}
              {...register('description')}
            ></textarea>
            {errors.description && <p className="text-red-500 text-xs">{errors.description.message}</p>}

            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                <div>
                    <input
                    type="date"
                    placeholder="Release year"
                    className={inputClasses}
                    {...register('releaseDate')}
                    />
                    {errors.releaseDate && <p className="text-red-500 text-xs">{errors.releaseDate.message}</p>}
                </div>
                <div>
                    <input
                    type="text"
                    placeholder="Running time in hh:mm:ss"
                    className={inputClasses}
                    {...register('runtime')}
                    />
                    {errors.runtime && <p className="text-red-500 text-xs">{errors.runtime.message}</p>}
                </div>
            </div>

            <div>
              <select
                className={inputClasses}
                {...register('productionCompanyId')}
              >
                <option value="">Choose production company</option>
                {productionCompanies.map(pc => (
                  <option key={pc.id} value={pc.id}>{pc.name}</option>
                ))}
              </select>
              {errors.productionCompanyId && <p className="text-red-500 text-xs">{errors.productionCompanyId.message}</p>}
            </div>

            <div>
                <ReactSelect
                    isMulti
                    options={genreOptions}
                    className="basic-multi-select"
                    classNamePrefix="select"
                    placeholder="Choose genre / genres"
                    value={selectedGenres}
                    onChange={(selected) => {
                        if (selected && Array.isArray(selected)) {
                            setValue('genreIds', selected.map(s => s.value));
                        } else {
                            setValue('genreIds', []);
                        }
                    }}
                    styles={customStyles}
                />
              {errors.genreIds && <p className="text-red-500 text-xs">{errors.genreIds.message}</p>}
            </div>
          </div>
        </div>

        <button
          type="submit"
          className="mt-6 w-full bg-blue-600 text-white py-3 px-4 rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
        >
          PUBLISH
        </button>
      </form>
    </div>
  );
};

export default MovieForm;