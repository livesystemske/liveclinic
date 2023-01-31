using System;
using System.Collections.Generic;
using System.Linq;
using LiveClinic.Billing.Data;
using LiveClinic.Billing.Domain;
using LiveClinic.Billing.ServicesRegistration;
using LiveClinic.Shared.Domain;
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
            SeedTestData(dbs);
        }
        
        private void SeedTestData(BillingDbContext context)
        {
            if (!context.Bills.Any())
            {
                context.Bills.AddRange(new List<Bill>
                {
                    new(1,"DOE, John",1),
                    new(2,"DOE, Mary",2)
                });
                context.SaveChanges();
            }
            
            if (!context.BillItems.Any())
            {
                context.BillItems.AddRange(new List<BillItem>
                {
                    new(1,1,Service.Registration,Money.From(5,Currency.USD),1),
                    new(2,2,Service.Registration,Money.From(5,Currency.USD),2),
                    new(1,3,Service.Consultation,Money.From(20,Currency.USD),3),
                });
                context.SaveChanges();
            }
            
            if (!context.Payments.Any())
            {
                context.Payments.AddRange(new List<Payment>
                {
                    new(1,Money.From(5,Currency.USD),1),
                    new(2,Money.From(5,Currency.USD),2)
                });
                context.SaveChanges();
            }
        }
    }
}