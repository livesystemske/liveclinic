using System;
using System.Collections.Generic;
using System.Linq;
using LiveClinic.Registry.Domain;
using LiveClinic.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace LiveClinic.Registry.Data
{
    public class RegistryDbContext:DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Encounter> Encounters { get; set; }
    
        public RegistryDbContext(DbContextOptions<RegistryDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .OwnsOne(p=>p.PatientName);
            
            base.OnModelCreating(modelBuilder);
        }

        public virtual void Seed()
        {
            if (!Patients.Any())
            {
                Patients.AddRange(new List<Patient>
                {
                    new(PersonName.From("John", "Doe"), Gender.Male, DateTime.Today.AddMonths(4).AddDays(7).AddYears(-47),1),
                    new(PersonName.From("Mary", "Doe"), Gender.Female, DateTime.Today.AddYears(-22),2)
                });
            }

            SaveChanges();
        }
    }
}