using Duende.Bff.Yarp;
using LiveClinic.Infrastructure.Data;
using LiveClinic.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveClinic.Infrastructure
{
    public static class RegisterInfrastructure
    {
        public static IServiceCollection RegisterAppInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddSqliteDatabase<LiveClinicDbContext>(configuration);
            services.SetupIdentity(configuration);
            return services;
        }
        
        private static IServiceCollection SetupIdentity(this IServiceCollection services,IConfiguration configuration)
        {
            // add BFF services and server-side session management
            services.AddBff(options =>
                {
                    options.EnableSessionCleanup = true;
                })
                .AddEntityFrameworkServerSideSessions(options=> 
                {
                    options.UseSqlite(configuration.GetConnectionString("LiveConnection"));        
                })
                .AddRemoteApis()
                .AddServerSideSessions();

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
                    options.Authority = "https://localhost/auth";
                    options.ClientId = "liveclinic.bff";
                    options.ClientSecret = "liveclinic.bff";
                    options.ResponseType = "code";
                    options.ResponseMode = "query";

                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;
                    options.MapInboundClaims = false;

                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("liveclinicbilling_api.read");
                    options.Scope.Add("liveclinicbilling_api.write");
                    options.Scope.Add("liveclinicregistry_api.read");
                    options.Scope.Add("liveclinicregistry_api.write");
                    options.Scope.Add("offline_access");

                    options.TokenValidationParameters.NameClaimType = "name";
                    options.TokenValidationParameters.RoleClaimType = "role";
                });
            
            return services;
        }
        
    }
}