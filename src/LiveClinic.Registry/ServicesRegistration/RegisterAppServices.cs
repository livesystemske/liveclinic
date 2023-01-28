using System.Reflection;
using LiveClinic.Registry.Data;
using LiveClinic.Shared;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveClinic.Registry.ServicesRegistration
{
    public static class RegisterAppServices
    {
        public static IServiceCollection RegisterApp(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSqliteDatabase<RegistryDbContext>(configuration);
            return services;
        }
    }
}