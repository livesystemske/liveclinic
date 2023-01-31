using LiveClinic.Billing.Infrastructure.Data;
using LiveClinic.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveClinic.Billing.Infrastructure
{
    public static class RegisterInfrastructure
    {
        public static IServiceCollection RegisterAppInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddSqliteDatabase<BillingDbContext>(configuration);
            return services;
        }
    }
}