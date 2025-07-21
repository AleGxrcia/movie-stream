using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MovieStream.Core.Application.Common.Behaviours;
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
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient<IFileManagerService, FileManagerService>();

            services.AddHttpContextAccessor();
        }
    }
}
