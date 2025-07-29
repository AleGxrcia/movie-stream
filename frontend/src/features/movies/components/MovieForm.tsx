import z from "zod";
import type { Movie } from "../types/movie.types";
import { useAppDispatch } from "../../../app/hooks";
import { useSelector } from "react-redux";
import { fetchProductionCompaniesAsync, selectProductionCompanies } from "../../productionCompanies/slices/productionCompaniesSlice";
import { fetchGenresAsync, selectGenres } from "../../genres/slices/genreSlice";
import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect } from "react";
import { useForm } from "react-hook-form";

const movieSchema = z.object({
  name: z.string().min(1, 'El nombre es requerido'),
  description: z.string().min(1, 'La descripción es requerida'),
  runtime: z.string().min(1, 'La duración es requerida'),
  releaseDate: z.string().min(1, 'La fecha de lanzamiento es requerida'),
  productionCompanyId: z.number().min(1, 'La productora es requerida'),
  genreIds: z.array(z.string().transform(Number)).min(1, 'Debes seleccionar al menos un género'),
  imageFile: z.instanceof(FileList).optional(),
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

  const { register, handleSubmit, formState: { errors }, setValue } = useForm<MovieFormValues>({
    resolver: zodResolver(movieSchema),
  });

  useEffect(() => {
    dispatch(fetchGenresAsync());
    dispatch(fetchProductionCompaniesAsync());
  }, [dispatch]);

  useEffect(() => {
    if (defaultValues) {
      setValue('name', defaultValues.name);
      setValue('description', defaultValues.description);
      setValue('runtime', defaultValues.runtime.toString());
      setValue('releaseDate', new Date(defaultValues.releaseDate).toISOString().split('T')[0]);
      setValue('productionCompanyId', defaultValues.productionCompany.id);
      setValue('genreIds', defaultValues.genres?.map(g => g.id) || []);
    }
  }, [defaultValues, setValue]);

  const handleFormSubmit = (data: MovieFormValues) => {
    const formData = new FormData();
    formData.append('name', data.name);
    formData.append('description', data.description);
    formData.append('runtime', data.runtime);
    formData.append('releaseDate', data.releaseDate);
    formData.append('productionCompanyId', data.productionCompanyId.toString());
    data.genreIds.forEach(id => formData.append('genreIds', id.toString()));
    if (data.imageFile && data.imageFile.length > 0) {
      formData.append('imageFile', data.imageFile[0]);
    }
    onSubmit(formData);
  };

  const onInvalid = (errors: any) => {
    console.error(errors);
  };

  return (
    <form onSubmit={handleSubmit(handleFormSubmit, onInvalid)} className="space-y-4">
      <div>
        <label htmlFor="name" className="block text-sm font-medium text-gray-700">Nombre</label>
        <input type="text" id="name" {...register('name')} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
        {errors.name && <p className="text-red-500 text-xs mt-1">{errors.name.message}</p>}
      </div>
      <div>
        <label htmlFor="description" className="block text-sm font-medium text-gray-700">Descripción</label>
        <textarea id="description" {...register('description')} rows={3} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"></textarea>
        {errors.description && <p className="text-red-500 text-xs mt-1">{errors.description.message}</p>}
      </div>
      <div>
        <label htmlFor="runtime" className="block text-sm font-medium text-gray-700">Duración (HH:mm:ss)</label>
        <input type="text" id="runtime" {...register('runtime')} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
        {errors.runtime && <p className="text-red-500 text-xs mt-1">{errors.runtime.message}</p>}
      </div>
      <div>
        <label htmlFor="releaseDate" className="block text-sm font-medium text-gray-700">Fecha de Lanzamiento</label>
        <input type="date" id="releaseDate" {...register('releaseDate')} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
        {errors.releaseDate && <p className="text-red-500 text-xs mt-1">{errors.releaseDate.message}</p>}
      </div>
      <div>
        <label htmlFor="productionCompanyId" className="block text-sm font-medium text-gray-700">Productora</label>
        <select id="productionCompanyId" {...register('productionCompanyId', { valueAsNumber: true })} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm">
          <option value="">Seleccione una productora</option>
          {productionCompanies.map(pc => <option key={pc.id} value={pc.id}>{pc.name}</option>)}
        </select>
        {errors.productionCompanyId && <p className="text-red-500 text-xs mt-1">{errors.productionCompanyId.message}</p>}
      </div>
      <div>
        <label className="block text-sm font-medium text-gray-700">Géneros</label>
        <div className="mt-2 space-y-2">
          {genres.map(genre => (
            <div key={genre.id} className="flex items-center">
              <input id={`genre-${genre.id}`} type="checkbox" value={genre.id} {...register('genreIds')} className="h-4 w-4 rounded border-gray-300 text-indigo-600 focus:ring-indigo-500" />
              <label htmlFor={`genre-${genre.id}`} className="ml-3 block text-sm text-gray-900">{genre.name}</label>
            </div>
          ))}
        </div>
        {errors.genreIds && <p className="text-red-500 text-xs mt-1">{errors.genreIds.message}</p>}
      </div>
      <div>
        <label htmlFor="imageFile" className="block text-sm font-medium text-gray-700">Imagen</label>
        <input type="file" id="imageFile" {...register('imageFile')} className="mt-1 block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-indigo-50 file:text-indigo-600 hover:file:bg-indigo-100" />
      </div>
      <button type="submit" className="inline-flex justify-center py-2 px-4 border border-transparent shadow-sm text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500">
        Guardar
      </button>
    </form>
  );
};

export default MovieForm;