using System.Threading.Tasks;
using LiveClinic.Billing.Application.Commands;
using LiveClinic.Billing.Application.Dtos;
using LiveClinic.Contracts;
using LiveClinic.Shared.Domain;
using MassTransit;
using MediatR;
using Serilog;

namespace LiveClinic.Billing.Application.EventHandlers
{
    public class BillingServicesConsumer:IConsumer<EncounterCreation>
    {
        private readonly IMediator _mediator;

        public BillingServicesConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<EncounterCreation> context)
        {
            Log.Information($"Recieved | {context.Message.PatientId} {context.Message.PatientName}");

            var billId = 0;

            var newb = new NewBillItemDto()
            {
                BillId =billId, EncounterId = context.Message.EncounterId, Service = (Service)context.Message.Service
            };
            var ress = await _mediator.Send(new AddBillItemCommand(newb));

        }
    }
}