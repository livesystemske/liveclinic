using System.Threading.Tasks;
using LiveClinic.Billing.Application.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LiveClinic.Billing.Tests.Application.Queries
{
    [TestFixture]
    public class GetPatientBillQueryTests
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
            var res =await _mediator.Send(new GetPatientBillQuery(1));
            Assert.That(res.IsSuccess,Is.True);
        }
    }
}