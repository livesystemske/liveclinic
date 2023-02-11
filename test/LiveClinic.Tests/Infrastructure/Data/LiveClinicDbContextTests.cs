using System.Linq;
using LiveClinic.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Tests.Infrastructure.Data
{
    [TestFixture]
    public class LiveClinicDbContextTests
    {
        private LiveClinicDbContext _context;
        [SetUp]
        public void Setup()
        {
            _context = TestInitializer.ServiceProvider.GetService<LiveClinicDbContext>();
        }

        [Test]
        public void should_Seed()
        {
            Assert.That(_context.Apps.Any(),Is.True);
            _context.Apps.ToList().ForEach(p=>Log.Information($"{p}"));
        }
    }
}