using System;
using System.Linq;
using LiveClinic.Registry.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace LiveClinic.Registry.ServicesRegistration
{
    public static class RegisterStartupMiddlewares
    {
        public static WebApplication SetupMiddleware(this WebApplication app)
        {
            app.UseForwardedHeaders();
            if (app.Environment.IsDevelopment())
            {
                var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });
            }

            //app.UseCors(RegisterStartupServices._policyName);
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.UseSerilogRequestLogging();
            SeedData(app);

            return app;
        }

        private static void SeedData(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<RegistryDbContext>();
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
