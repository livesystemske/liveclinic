using System.Linq;
using System.Threading.Tasks;
using LiveClinic.Billing.Application.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LiveClinic.Billing.Tests.Application.Queries
{
    [TestFixture]
    public class GetBillsQueryTests
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
            var res =await _mediator.Send(new GetBillsQuery());
            Assert.That(res.IsSuccess,Is.True);
            Assert.That(res.Value.Any(),Is.True);
        }
    }
}