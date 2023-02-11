using System.Threading;
using System.Threading.Tasks;
using LiveClinic.Contracts;
using LiveClinic.Registry.Domain.Events;
using MassTransit;
using MediatR;
using Serilog;

namespace LiveClinic.Registry.Application.EventHandlers
{
    public class PatientRegisteredEventHandler : INotificationHandler<PatientRegisteredEvent>
    {
        private readonly IBus _bus;

        public PatientRegisteredEventHandler(IBus bus, IMediator mediator)
        {
            _bus = bus;
        }

        public async Task Handle(PatientRegisteredEvent notification, CancellationToken cancellationToken)
        {
            Log.Information(
                $"Publishing {nameof(notification.GetType)} <<[{notification.PatientId},{notification.PatientName}]>>");

            await _bus.Publish(new PatientRegistration
            {
                PatientId = notification.PatientId,
                PatientName = notification.PatientName,
                EncounterId = notification.EncounterId
            }, cancellationToken);
        }
    }
}