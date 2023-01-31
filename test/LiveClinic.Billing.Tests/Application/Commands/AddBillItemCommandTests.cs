using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Billing.Application.Commands;
using LiveClinic.Billing.Application.Dtos;
using LiveClinic.Shared.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LiveClinic.Billing.Tests.Application.Commands
{
    [TestFixture]
    public class AddBillItemCommandTests : IRequest<Result>
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
            var dto = new NewBillItemDto()
            {
                BillId = 1, EncounterId = 5,Service = Service.Lab
            };
            var res =await _mediator.Send(new AddBillItemCommand(dto));
            Assert.That(res.IsSuccess,Is.True);
        }
    }
}