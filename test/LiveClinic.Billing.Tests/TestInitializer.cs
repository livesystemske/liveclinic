using System;
using LiveClinic.Billing.Data;
using LiveClinic.Billing.ServicesRegistration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Billing.Tests
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
            services.RegisterApp(config);
            ServiceProvider = services.BuildServiceProvider();
        }

        private void InitDb()
        {
            var dbs = ServiceProvider.GetService<BillingDbContext>();
            dbs.Database.EnsureCreated();
            dbs.Seed();
        }
    }
}