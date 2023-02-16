using System;
using CSharpFunctionalExtensions;
using LiveClinic.Registry.Application.Commands;
using LiveClinic.Registry.Application.Dtos;
using LiveClinic.Shared.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LiveClinic.Registry.Tests.Application.Commands
{
    public class RegisterPatientCommandTests:IRequest<Result>
    {
        private IMediator _mediator;
        private int _fix;
        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
            _fix = new Random().Next(1, 9999);
        }
        [Test]
        public void should_Read()
        {
            var dto = new NewPatientDto()
            {
                FirstName = $"FName{_fix}", LastName = $"LName{_fix}", Gender = Gender.Female
            };
            var res = _mediator.Send(new RegisterPatientCommand(dto));
            Assert.That(res.Result.IsSuccess,Is.True);
        }
    }
}