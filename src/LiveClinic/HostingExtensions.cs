using System;
using System.Collections.Generic;
using Duende.Bff.Yarp;
using LiveClinic.Shared.Common.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace LiveClinic
{
    internal static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            var apiServices = builder.Configuration.GetSection(ApiServiceSetting.Key).Get<List<ApiServiceSetting>>();
            builder.Services.AddSingleton(apiServices);
            builder.Services.AddRazorPages();

            builder.Services.AddControllers();

            // add BFF services and server-side session management
            builder.Services.AddBff()
                .AddRemoteApis()
                .AddServerSideSessions();

            builder.Services.AddAuthentication(options =>
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
                    options.Authority = "https://demo.duendesoftware.com";
                    options.ClientId = "interactive.confidential";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";
                    options.ResponseMode = "query";

                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;
                    options.MapInboundClaims = false;

                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("api");
                    options.Scope.Add("offline_access");

                    options.TokenValidationParameters.NameClaimType = "name";
                    options.TokenValidationParameters.RoleClaimType = "role";
                });

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();

            // add CSRF protection and status code handling for API endpoints
            // app.UseBff();
            app.UseAuthorization();

            SetupApiServices(app);
            
            // local API endpoints
            app.MapControllers()
                //.RequireAuthorization()
                .AsBffApiEndpoint();

          

            return app;
        }

        private static void SetupApiServices(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var apiServices = scope.ServiceProvider.GetService<List<ApiServiceSetting>>();

                app.MapBffManagementEndpoints();
                Log.Information($"Initializing Services...");
                // enable proxying to remote API
                apiServices.ForEach(s =>
                {
                    app.MapRemoteBffApiEndpoint(s.LocalPath, s.ApiAddress);
                    Log.Information($"Initializing Service [{s.Name}]");
                    // .RequireAccessToken();    
                });
                
                Log.Information($"Initialized [{apiServices.Count}] Services !");
            }
        }
    }
}
