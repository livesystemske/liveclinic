using LiveClinic.Registry.Domain;
using Microsoft.EntityFrameworkCore;

namespace LiveClinic.Registry.Data
{
    public class RegistryDbContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Encounter> Encounters { get; set; }

        public RegistryDbContext(DbContextOptions<RegistryDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .OwnsOne(p => p.PatientName);

            base.OnModelCreating(modelBuilder);
        }

        public virtual void Seed()
        {
        }
    }
}