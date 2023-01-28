using System.Reflection;
using LiveClinic.Billing.Data;
using LiveClinic.Shared;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveClinic.Billing.ServicesRegistration
{
    public static class RegisterAppServices
    {
        public static IServiceCollection RegisterApp(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSqliteDatabase<BillingDbContext>(configuration);
            return services;
        }
    }
}