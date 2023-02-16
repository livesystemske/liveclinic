using System.Threading;
using System.Threading.Tasks;
using LiveClinic.Contracts;
using LiveClinic.Registry.Domain.Events;
using MassTransit;
using MediatR;
using Serilog;

namespace LiveClinic.Registry.Application.EventHandlers
{
    public class EncounterCreatedEventHandler : INotificationHandler<EncounterCreatedEvent>
    {
        private readonly IBus _bus;

        public EncounterCreatedEventHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(EncounterCreatedEvent notification, CancellationToken cancellationToken)
        {
            Log.Information(
                $"Publishing {nameof(notification.GetType)} <<[{notification.EncounterId},{notification.PatientName}]>>");
            await _bus.Publish(new EncounterCreation()
            {
                PatientId = notification.PatientId,
                PatientName = notification.PatientName,
                EncounterId = notification.EncounterId,
                Service = (int)notification.Service
            }, cancellationToken);
        }
    }
}