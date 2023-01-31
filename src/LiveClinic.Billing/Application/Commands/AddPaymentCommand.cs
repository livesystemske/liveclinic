using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Billing.Application.Dtos;
using LiveClinic.Billing.Domain;
using LiveClinic.Billing.Domain.Events;
using LiveClinic.Billing.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LiveClinic.Billing.Application.Commands
{
    public class AddPaymentCommand : IRequest<Result>
    {
        public NewPaymentDto NewPayment { get; }

        public AddPaymentCommand(NewPaymentDto newPayment)
        {
            NewPayment = newPayment;
        }
    }

    public class AddPaymentCommandHandler : IRequestHandler<AddPaymentCommand, Result>
    {
        private readonly IMediator _mediator;
        private readonly BillingDbContext _context;

        public AddPaymentCommandHandler(IMediator mediator, BillingDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task<Result> Handle(AddPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var bill = _context
                    .Bills.AsNoTracking()
                    .Include(i=>i.Items)
                    .Include(x=>x.Payments)
                    .FirstOrDefault(x => x.Id == request.NewPayment.BillId);
                
                if (null == bill)
                    throw new ArgumentException("Bill not found!");
                
                if (bill.IsAlreadyPaid)
                    throw new ArgumentException("Bill is already paid!");
                
                var payment = Payment.From(request.NewPayment);
                await _context.AddAsync(payment,cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await _mediator.Publish(new PaymentAddedEvent(payment.Id), cancellationToken);
                return Result.Success();
            }
            catch (Exception e)
            {
                Log.Error(e, "{Name} error!", request.GetType().Name);
                return Result.Failure(e.Message);
            }
        }
    }
}