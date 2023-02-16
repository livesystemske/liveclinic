using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Duende.Bff.Yarp;
using LiveClinic.Infrastructure.Data;
using LiveClinic.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveClinic.Infrastructure
{
    public static class RegisterInfrastructure
    {
        public static IServiceCollection RegisterAppInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSqliteDatabase<LiveClinicDbContext>(configuration);
            services.SetupIdentity(configuration);
            return services;
        }

        private static IServiceCollection SetupIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            // add BFF services and server-side session management
            services.AddBff(options => { options.EnableSessionCleanup = true; })
                .AddEntityFrameworkServerSideSessions(options =>
                {
                    options.UseSqlite(configuration.GetConnectionString("LiveConnection"));
                })
                .AddRemoteApis()
                .AddServerSideSessions();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "cookie";
                    options.DefaultChallengeScheme = "oidc";
                    options.DefaultSignOutScheme = "oidc";
                })
                .AddCookie("cookie", options =>
                {
                    options.Cookie.Name = "__Host-bff";
                    options.Cookie.SameSite = SameSiteMode.Strict;
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "https://localhost/liveclinicauth";
                    options.ClientId = "liveclinic";
                    // options.ClientSecret = "liveclinic.bff";
                    options.ResponseType = "code";
                    options.ResponseMode = "query";

                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;
                    options.MapInboundClaims = false;

                    options.Scope.Clear();
                    new List<string>()
                    {
                        "registry.read", "registry.manage", 
                        "billing.read", "billing.manage", 
                        "roles", "openid","profile","offline_access"
                    }.ForEach(
                        scope => { options.Scope.Add(scope); });

                    options.TokenValidationParameters.NameClaimType = "name";
                    options.TokenValidationParameters.RoleClaimType = "role";
                });

            return services;
        }
    }
}