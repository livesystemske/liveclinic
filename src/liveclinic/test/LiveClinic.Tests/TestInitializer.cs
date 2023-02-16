using System;
using System.Linq;
using LiveClinic.Application;
using LiveClinic.Domain;
using LiveClinic.Infrastructure;
using LiveClinic.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Tests
{
    [SetUpFixture]
    public class TestInitializer
    {
        public static IServiceProvider ServiceProvider;

        [OneTimeSetUp]
        public void Init()
        {
            SetupDependencyInjection();
            InitDb();
        }

        private void SetupDependencyInjection()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            var services = new ServiceCollection();
            
            services.RegisterApplicationServices(config);
            services.RegisterAppInfrastructure(config);
            ServiceProvider = services.BuildServiceProvider();
        }

        private void InitDb()
        {
            var dbs = ServiceProvider.GetService<LiveClinicDbContext>();
            dbs.Database.EnsureCreated();
            dbs.Seed();
            SeedTestData(dbs);
        }
        
        private void SeedTestData(LiveClinicDbContext context)
        {
            if (!context.Apps.Any())
            {
                context.Apps.AddRange(Enumerable.Range(1,5).Select(x=>new LiveApp($"App {x}")));
                context.SaveChanges();
            }
        }
    }
}
