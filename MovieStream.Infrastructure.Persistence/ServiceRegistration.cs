using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieStream.Core.Application.Interfaces.Repositories;
using MovieStream.Infrastructure.Persistence.Contexts;
using MovieStream.Infrastructure.Persistence.Repositories;

namespace MovieStream.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            #region "Contexts configurations"

            if (config.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("AppDb"));
            }
            else
            {
                var connectionString = config.GetConnectionString("DefaultConnection");
                services.AddDbContext<ApplicationDbContext>(options =>
                                                    options.UseSqlServer(connectionString,
                                                    m => m.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            #endregion

            #region Repositories

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<ITvSerieRepository, TvSerieRepository>();
            services.AddTransient<IMovieRepository, MovieRepository>();
            services.AddTransient<ISeasonRepository, SeasonRepository>();
            services.AddTransient<IEpisodeRepository, EpisodeRepository>();
            services.AddTransient<IGenreRepository, GenreRepository>();
            services.AddTransient<IProductionCompanyRepository, ProductionCompanyRepository>();

            #endregion
        }
    }
}
