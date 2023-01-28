using System;
using System.Collections.Generic;
using System.Linq;
using LiveClinic.Billing.Domain;
using LiveClinic.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace LiveClinic.Billing.Data
{
    public class BillingDbContext:DbContext
    {
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillItem> BillItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ServicePrice> ServicePrices { get; set; }
    
        public BillingDbContext(DbContextOptions<BillingDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BillItem>()
                .OwnsOne(x => x.Charge);
        
            modelBuilder.Entity<Payment>()
                .OwnsOne(x => x.Amount);
        
            modelBuilder.Entity<ServicePrice>()
                .OwnsOne(x => x.Price);
        
            base.OnModelCreating(modelBuilder);
        }
        
        public virtual void Seed()
        {
            if (!ServicePrices.Any())
            {
                ServicePrices.AddRange(new List<ServicePrice>
                {
                    new(Service.Registration,Money.From(5,Currency.USD),DateTime.Now.AddYears(1)),
                    new(Service.Consultation,Money.From(20,Currency.USD),DateTime.Now.AddYears(1)),
                    new(Service.Pharmacy,Money.From(15,Currency.USD),DateTime.Now.AddYears(1)),
                    new(Service.Lab,Money.From(10,Currency.USD),DateTime.Now.AddYears(1))
                });
            }

            SaveChanges();
        }
    }
}