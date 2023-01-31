using LiveClinic.Registry.Data;
using LiveClinic.Shared;
using LiveClinic.Shared.Common;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveClinic.Registry.Infrastructure
{
    public static class RegisterInfrastructure
    {
        public static IServiceCollection RegisterAppInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddSqliteDatabase<RegistryDbContext>(configuration);
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