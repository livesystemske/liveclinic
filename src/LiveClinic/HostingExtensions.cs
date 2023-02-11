using System;
using System.Collections.Generic;
using Duende.Bff;
using Duende.Bff.Yarp;
using LiveClinic.Application;
using LiveClinic.Infrastructure;
using LiveClinic.Infrastructure.Data;
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

            builder.Services.RegisterAppInfrastructure(builder.Configuration);
            builder.Services.RegisterApplicationServices(builder.Configuration);
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
            app.UseBff();
            app.UseAuthorization();

            SetupApiServices(app);
            
            // local API endpoints
            app.MapControllers()
                .RequireAuthorization()
                .AsBffApiEndpoint();
            SeedData(app);
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
                    Log.Information($"Initializing Service [{s.Name}]");

                    app.MapRemoteBffApiEndpoint(s.LocalPath, s.ApiAddress)
                        .RequireAccessToken(TokenType.UserOrClient);
                    //.SkipResponseHandling();
                });
                
                Log.Information($"Initialized [{apiServices.Count}] Services !");
            }
        }
        
        private static void SeedData(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<LiveClinicDbContext>();
                try
                {
                    context.Database.EnsureCreated();
                    context.Seed();
                    Log.Debug($"initializing Database [OK]");
                }
                catch (Exception e)
                {
                    Log.Error(e,$"initializing Database Error");
                }
            }
        }
    }
}
