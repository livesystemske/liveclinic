using System.Threading.Tasks;
using LiveClinic.Billing.Application.Commands;
using LiveClinic.Billing.Application.Dtos;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LiveClinic.Billing.Tests.Application.Commands
{
    [TestFixture]
    public class GenerateBillCommandTests
    {
        private IMediator _mediator;
        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }
        [Test]
        public async Task should_Generate()
        {
            var dto = new NewBillDto()
            {
                PatientId = 1, PatientName = "DOE, John"
            };
            var res =await _mediator.Send(new GenerateBillCommand(dto));
            Assert.That(res.IsSuccess,Is.True);
        }
    }
}