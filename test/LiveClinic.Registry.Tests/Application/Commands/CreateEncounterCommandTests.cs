using CSharpFunctionalExtensions;
using LiveClinic.Registry.Application.Commands;
using LiveClinic.Registry.Application.Dtos;
using LiveClinic.Shared.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LiveClinic.Registry.Tests.Application.Commands
{
    public class CreateEncounterCommandTests:IRequest<Result>
    {
        private IMediator _mediator;
        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }
        [Test]
        public void should_Read()
        {
            var dto = new NewEncounterDto()
            {
                PatientId = 1,Service=Service.Pharmacy
            };
            var res = _mediator.Send(new CreateEncounterCommand(dto));
            Assert.That(res.Result.IsSuccess,Is.True);
        }
    }
}