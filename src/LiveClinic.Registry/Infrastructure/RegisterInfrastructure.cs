using LiveClinic.Registry.Data;
using LiveClinic.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveClinic.Registry.Infrastructure
{
    public static class RegisterInfrastructure
    {
        public static IServiceCollection RegisterAppInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddSqliteDatabase<RegistryDbContext>(configuration);
            return services;
        }
    }
}