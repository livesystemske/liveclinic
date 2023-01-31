using System;
using LiveClinic.Registry.Application;
using LiveClinic.Registry.Infrastructure;
using LiveClinic.Shared.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace LiveClinic.Registry.ServicesRegistration
{
    public static class RegisterStartupServices
    {
        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
        {
            var environment = builder.Environment.EnvironmentName;
            
            builder.Configuration.
                AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("serilog.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"serilog.{environment}.json", optional: true, reloadOnChange: true);
            
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();
            
            builder.Host.UseSerilog((hostContext, loggerConfig) =>
            {
                loggerConfig
                    .ReadFrom.Configuration(hostContext.Configuration)
                    .Enrich.WithProperty("ApplicationName", hostContext.HostingEnvironment.ApplicationName);
            });
            
            var liveAuthSetting = builder.Configuration.GetSection(LiveAuthSetting.Key).Get<LiveAuthSetting>();
            builder.Services.AddSingleton(liveAuthSetting);
            var transportSetting = builder.Configuration.GetSection(TransportSetting.Key).Get<TransportSetting>();
            builder.Services.AddSingleton(transportSetting);
            builder.Services.Configure<LiveAuthSetting>(builder.Configuration.GetSection(LiveAuthSetting.Key));
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = liveAuthSetting.Authority;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = SetUpFlow(liveAuthSetting)
                });
                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
            
            builder.Services.RegisterApplicationServices(builder.Configuration);
            builder.Services.RegisterAppInfrastructure(builder.Configuration);
            
            return builder;
        }

        private static OpenApiOAuthFlows SetUpFlow(LiveAuthSetting liveAuthSetting)
        {
            if (liveAuthSetting.Flow == "AuthorizationCode")
                return new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{liveAuthSetting.Authority}/connect/authorize"),
                        TokenUrl = new Uri($"{liveAuthSetting.Authority}/connect/token"),
                        Scopes = liveAuthSetting.Scopes
                    }
                };

            return new OpenApiOAuthFlows
            {
                ClientCredentials = new OpenApiOAuthFlow()
                {
                    AuthorizationUrl = new Uri($"{liveAuthSetting.Authority}/connect/authorize"),
                    TokenUrl = new Uri($"{liveAuthSetting.Authority}/connect/token"),
                    Scopes = liveAuthSetting.Scopes
                }
            };
        }
    }
}