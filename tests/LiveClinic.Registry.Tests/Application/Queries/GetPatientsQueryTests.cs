using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveClinic.Registry.Application.Queries;
using LiveClinic.Registry.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LiveClinic.Registry.Tests.Application.Queries
{
    [TestFixture]
    public class GetPatientsQueryTests:IRequest<List<Patient>>
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
            var res =await _mediator.Send(new GetPatientsQuery());
            Assert.That(res.IsSuccess,Is.True);
            Assert.That(res.Value.Any(),Is.True);
        }
    }
}