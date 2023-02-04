using System.Reflection;
using LiveClinic.Billing.Infrastructure.Data;
using LiveClinic.Shared;
using LiveClinic.Shared.Common;
using LiveClinic.Shared.Common.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveClinic.Billing.Infrastructure
{
    public static class RegisterInfrastructure
    {
        public static IServiceCollection RegisterAppInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddSqliteDatabase<BillingDbContext>(configuration);
            services.SetupTransport(configuration);
            return services;
        }
        
        private static IServiceCollection SetupTransport(this IServiceCollection services,IConfiguration configuration)
        {
            
            var transportSetting = new TransportSetting(
                configuration.GetValue<string>($"{TransportSetting.Key}:{nameof(TransportSetting.Mode)}"),
                configuration.GetValue<string>($"{TransportSetting.Key}:{nameof(TransportSetting.Host)}"),
                configuration.GetValue<string>($"{TransportSetting.Key}:{nameof(TransportSetting.VHost)}"),
                configuration.GetValue<string>($"{TransportSetting.Key}:{nameof(TransportSetting.User)}"),
            configuration.GetValue<string>($"{TransportSetting.Key}:{nameof(TransportSetting.Password)}")
            );
         
            if (transportSetting.Mode == "RabbitMQ")
            {
                services.AddMassTransit(x =>
                {
                    x.AddConsumers(Assembly.GetExecutingAssembly());
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(transportSetting.Host, transportSetting.VHost, h =>
                        {
                            h.Username(transportSetting.User);
                            h.Password(transportSetting.Password);
                        });

                        cfg.ConfigureEndpoints(context);
                    });
                });
            }
           
            return services;
        }
    }
}