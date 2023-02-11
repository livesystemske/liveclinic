using LiveClinic.Registry.Data;
using LiveClinic.Shared;
using LiveClinic.Shared.Common;
using LiveClinic.Shared.Common.Settings;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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
        
        private static IServiceCollection SetupIdentity(this IServiceCollection services,IConfiguration configuration)
        {
            var transportSetting = new TransportSetting(
                configuration.GetValue<string>($"{TransportSetting.Key}:{nameof(TransportSetting.Mode)}"),
                configuration.GetValue<string>($"{TransportSetting.Key}:{nameof(TransportSetting.Host)}"),
                configuration.GetValue<string>($"{TransportSetting.Key}:{nameof(TransportSetting.VHost)}"),
                configuration.GetValue<string>($"{TransportSetting.Key}:{nameof(TransportSetting.User)}"),
                configuration.GetValue<string>($"{TransportSetting.Key}:{nameof(TransportSetting.Password)}")
            );

            var authSettings = new LiveAuthSetting(
                configuration.GetValue<string>($"{LiveAuthSetting.Key}:{nameof(LiveAuthSetting.Authority)}"),
                configuration.GetValue<string>($"{LiveAuthSetting.Key}:{nameof(LiveAuthSetting.ClientId)}"),
                configuration.GetValue<string>($"{LiveAuthSetting.Key}:{nameof(LiveAuthSetting.Secret)}"),
                configuration.GetValue<string>($"{LiveAuthSetting.Key}:{nameof(LiveAuthSetting.Scope)}")
            );

            services.AddAuthentication(opts =>
                {
                    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = authSettings.Authority;
                    options.RequireHttpsMetadata = false;
                    options.Audience = authSettings.Scope;
                });

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
