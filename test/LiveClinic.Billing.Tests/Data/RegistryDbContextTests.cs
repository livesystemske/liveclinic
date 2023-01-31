using System.Linq;
using LiveClinic.Billing.Data;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Billing.Tests.Data
{
    [TestFixture]
    public class BillingDbContextTests
    {
        private BillingDbContext _context;
        [SetUp]
        public void Setup()
        {
            _context = TestInitializer.ServiceProvider.GetService<BillingDbContext>();
        }

        [Test]
        public void should_Seed()
        {
            Assert.That(_context.ServicePrices.Any(),Is.True);
            Assert.That(_context.Bills.Any(),Is.True);
            Assert.That(_context.BillItems.Any(),Is.True);
            Assert.That(_context.Payments.Any(),Is.True);
            _context.ServicePrices.ToList().ForEach(p=>Log.Information($"{p}"));
        }
    }
}