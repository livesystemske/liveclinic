using System.Threading.Tasks;
using LiveClinic.Billing.Application.Commands;
using LiveClinic.Billing.Application.Dtos;
using LiveClinic.Shared.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LiveClinic.Billing.Tests.Application.Commands
{
    [TestFixture]
    public class AddPaymentCommandTests
    { 
        private IMediator _mediator;
        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }
        [Test]
        public async Task should_Add()
        {
            var dto = new NewPaymentDto()
            {
                BillId = 1,Amount = 20,Currency = Currency.USD
            };
            var res =await _mediator.Send(new AddPaymentCommand(dto));
            Assert.That(res.IsSuccess,Is.True);
        }
    }
}