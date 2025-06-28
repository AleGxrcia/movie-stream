using Microsoft.Extensions.DependencyInjection;
using MovieStream.Core.Application.Interfaces.Services;
using MovieStream.Core.Application.Services;
using System.Reflection;

namespace MovieStream.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddTransient<IFileManagerService, FileManagerService>();
        }
    }
}
