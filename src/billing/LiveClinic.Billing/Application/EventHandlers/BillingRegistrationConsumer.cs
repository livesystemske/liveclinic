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
    public class BillingRegistrationConsumer:IConsumer<PatientRegistration>
    {
        private readonly IMediator _mediator;

        public BillingRegistrationConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<PatientRegistration> context)
        {
            Log.Information($"Recieved | {context.Message.PatientId} {context.Message.PatientName}");
            var newBill = new NewBillDto()
                { PatientId = context.Message.PatientId, PatientName = context.Message.PatientName };
        
            var res = await _mediator.Send(new GenerateBillCommand(newBill));
        
            if (res.IsSuccess)
            {
                var newb = new NewBillItemDto()
                {
                    BillId = res.Value, EncounterId = context.Message.EncounterId, Service = Service.Registration
                };
                var ress = await _mediator.Send(new AddBillItemCommand(newb));
            }
            
        }
    }
}