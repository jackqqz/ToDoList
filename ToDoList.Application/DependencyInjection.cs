using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;

namespace ToDoList.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            // Add application services here, e.g., MediatR, AutoMapper, etc.
            return services;
        }
    }
}