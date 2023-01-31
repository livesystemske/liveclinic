using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Billing.Application.Dtos;
using LiveClinic.Billing.Data;
using LiveClinic.Billing.Domain;
using LiveClinic.Billing.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LiveClinic.Billing.Application.Commands
{
    public class AddBillItemCommand : IRequest<Result>
    {
        public NewBillItemDto NewBillItem { get; }

        public AddBillItemCommand(NewBillItemDto newBillItem)
        {
            NewBillItem = newBillItem;
        }
    }

    public class AddBillItemCommandHandler : IRequestHandler<AddBillItemCommand, Result>
    {
        private readonly IMediator _mediator;
        private readonly BillingDbContext _context;

        public AddBillItemCommandHandler(IMediator mediator, BillingDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task<Result> Handle(AddBillItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var servicePrice = _context
                    .ServicePrices.AsNoTracking()
                    .FirstOrDefault(x => x.Service == request.NewBillItem.Service);
                
                if (null == servicePrice)
                    throw new ArgumentException("Service price not found!");
                
                var bill = _context
                    .Bills.AsNoTracking()
                    .FirstOrDefault(x => x.Id == request.NewBillItem.BillId);
                
                if (null == bill)
                    throw new ArgumentException("Bill not found!");

                var billItem = BillItem.From(request.NewBillItem, servicePrice.Price);
                await _context.AddAsync(billItem,cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await _mediator.Publish(new BillItemCreatedEvent(billItem.Id), cancellationToken);
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