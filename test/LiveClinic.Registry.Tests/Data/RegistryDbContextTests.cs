using System.Linq;
using LiveClinic.Registry.Data;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Registry.Tests.Data
{
    [TestFixture]
    public class RegistryDbContextTests
    {
        private RegistryDbContext _context;
        [SetUp]
        public void Setup()
        {
            _context = TestInitializer.ServiceProvider.GetService<RegistryDbContext>();
        }

        [Test]
        public void should_Seed()
        {
            Assert.That(_context.Patients.Any(),Is.True);
            Assert.That(_context.Encounters.Any(),Is.True);
            _context.Patients.ToList().ForEach(p=>Log.Information($"{p}"));
        }
    }
}