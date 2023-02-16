using System.Collections.Generic;
using System.Linq;
using LiveClinic.Domain;
using Microsoft.EntityFrameworkCore;

namespace LiveClinic.Infrastructure.Data
{
    public class LiveClinicDbContext:DbContext
    {
        public DbSet<LiveApp> Apps { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public LiveClinicDbContext(DbContextOptions<LiveClinicDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        
        public virtual void Seed()
        {
            if (!Apps.Any())
            {
                Apps.AddRange(new List<LiveApp>
                {
                    new("Registry"),
                    new("Billing")
                });
            }

            SaveChanges();
        }
    }
}