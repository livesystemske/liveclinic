using System;
using LiveClinic.Registry.Application;
using LiveClinic.Registry.Infrastructure;
using LiveClinic.Shared.Common;
using LiveClinic.Shared.Common.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;

namespace LiveClinic.Registry.ServicesRegistration
{
    public static class RegisterStartupServices
    {
        public static readonly string _policyName = "CorsPolicy";
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
            
            builder.Services.AddApiVersioning(apiVerConfig =>
            {
                apiVerConfig.AssumeDefaultVersionWhenUnspecified = true;
                apiVerConfig.DefaultApiVersion = new ApiVersion(1, 0);
                apiVerConfig.ReportApiVersions = true;
                apiVerConfig.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("x-api-version"),
                    new MediaTypeApiVersionReader("x-api-version")
                );
            });
            // Add ApiExplorer to discover versions
            builder.Services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy(name: _policyName, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = SetUpFlow(builder.Configuration.GetSection(LiveAuthSetting.Key).Get<LiveAuthSetting>())
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
