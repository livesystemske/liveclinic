﻿using System;
using System.Collections.Generic;
using System.Linq;
using LiveClinic.Registry.Application;
using LiveClinic.Registry.Data;
using LiveClinic.Registry.Domain;
using LiveClinic.Registry.Infrastructure;
using LiveClinic.Registry.ServicesRegistration;
using LiveClinic.Shared.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Registry.Tests
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
            var dbs = ServiceProvider.GetService<RegistryDbContext>();
            dbs.Database.EnsureCreated();
            dbs.Seed();
            SeedTestData(dbs);
        }

        private void SeedTestData(RegistryDbContext context)
        {
            if (!context.Patients.Any())
            {
                context.Patients.AddRange(new List<Patient>
                {
                    new(PersonName.From("John", "Doe"), Gender.Male, DateTime.Today.AddMonths(4).AddDays(7).AddYears(-47),1),
                    new(PersonName.From("Mary", "Doe"), Gender.Female, DateTime.Today.AddYears(-22),2)
                });
                context.SaveChanges();
            }
            
            if (!context.Encounters.Any())
            {
                context.Encounters.AddRange(new List<Encounter>
                {
                    new Encounter(1,Service.Registration,1),
                    new Encounter(2,Service.Registration,2),
                    new Encounter(1,Service.Consultation,3),
                    new Encounter(2,Service.Consultation,4),
                    new Encounter(2,Service.Lab,5)
                });
                context.SaveChanges();
            }
        }
    }
}