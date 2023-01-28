using System;
using LiveClinic.Billing.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace LiveClinic.Billing.ServicesRegistration
{
    public static class RegisterStartupMiddlewares
    {
        public static WebApplication SetupMiddleware(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            SeedData(app);
            return app;
        }
        
        private static void SeedData(WebApplication app)
        {
            var context = app.Services.GetService<BillingDbContext>();
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