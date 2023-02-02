using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveClinic.Billing.Application;
using LiveClinic.Billing.Domain;
using LiveClinic.Billing.Infrastructure;
using LiveClinic.Billing.Infrastructure.Data;
using LiveClinic.Billing.ServicesRegistration;
using LiveClinic.Contracts;
using LiveClinic.Shared.Domain;
using MassTransit;
using MassTransit.Testing;
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
            
            services.RegisterApplicationServices(config);
            services.RegisterAppInfrastructure(config);
            services.AddMassTransitTestHarness(cfg =>{cfg.AddConsumer<TestConsumer>();});
            ServiceProvider = services.BuildServiceProvider();
        }

        private void InitDb()
        {
            var dbs = ServiceProvider.GetService<BillingDbContext>();
            dbs.Database.EnsureCreated();
            dbs.Seed();
            SeedTestData(dbs);
            var harness = ServiceProvider.GetRequiredService<ITestHarness>();
            harness.Start().Wait();
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
        
        public class TestConsumer:IConsumer<PatientRegistration>,IConsumer<EncounterCreation>
        {
            public  Task Consume(ConsumeContext<PatientRegistration> context)
            {
                Log.Information($"Recieved | {context.Message.PatientId} {context.Message.PatientName}");
                return Task.CompletedTask;
            }

            public  Task Consume(ConsumeContext<EncounterCreation> context)
            {
                Log.Information($"Recieved | {context.Message.PatientId} {context.Message.PatientName}");
                return Task.CompletedTask;
            }
        }
    }
}
