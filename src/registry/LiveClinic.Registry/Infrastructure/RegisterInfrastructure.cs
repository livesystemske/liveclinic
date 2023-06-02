﻿using System.Linq;
using LiveClinic.Registry.Infrastructure.Data;
using LiveClinic.Shared;
using LiveClinic.Shared.Common.Settings;
using MassTransit;
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
            services.SetupIdentity(configuration);
            services.SetupTransport(configuration);
            return services;
        }

        private static IServiceCollection SetupIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            var authSettings = configuration.GetSection(LiveAuthSetting.Key).Get<LiveAuthSetting>();
            services.AddSingleton(authSettings);

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = authSettings.Authority;
                    options.Audience = "registry";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            var policySetting = configuration.GetSection(PolicySetting.Key).Get<PolicySetting>();
            services.AddSingleton(policySetting);
           
            if (null != policySetting && policySetting.Definitions.Any())
                services.AddAuthorization(options =>
                {
                    foreach (var definition in policySetting.Definitions)
                    {
                        options.AddPolicy(definition.Name,
                            policy => policy.RequireClaim(
                                "permission", definition.Permissions
                            ));
                    }
                });

            return services;
        }

        private static IServiceCollection SetupTransport(this IServiceCollection services,IConfiguration configuration)
        {
            var transportSetting=configuration.GetSection(TransportSetting.Key).Get<TransportSetting>();
            services.AddSingleton(transportSetting);
            
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