using System.Linq;
using System.Threading.Tasks;
using LiveClinic.Application.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LiveClinic.Tests.Application.Queries
{
    [TestFixture]
    public class GetLiveAppsQueryTests
    {
        private IMediator _mediator;
        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }
        [Test]
        public async Task should_Read()
        {
            var res =await _mediator.Send(new GetLiveAppsQuery());
            Assert.That(res.IsSuccess,Is.True);
            Assert.That(res.Value.Any(),Is.True);
        }
    }
}