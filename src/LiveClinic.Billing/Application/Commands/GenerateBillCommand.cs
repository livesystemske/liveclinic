using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Billing.Application.Dtos;
using LiveClinic.Billing.Domain;
using LiveClinic.Billing.Domain.Events;
using LiveClinic.Billing.Infrastructure.Data;
using MediatR;
using Serilog;

namespace LiveClinic.Billing.Application.Commands
{
    public class GenerateBillCommand : IRequest<Result<long>>
    {
        public NewBillDto NewBill { get; }

        public GenerateBillCommand(NewBillDto newBill)
        {
            NewBill = newBill;
        }
    }

    public class GenerateBillCommandHandler : IRequestHandler<GenerateBillCommand, Result<long>>
    {
        private readonly IMediator _mediator;
        private readonly BillingDbContext _context;

        public GenerateBillCommandHandler(IMediator mediator, BillingDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task<Result<long>> Handle(GenerateBillCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var bill = Bill.From(request.NewBill);
                await _context.AddAsync(bill, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(bill.Id);
            }
            catch (Exception e)
            {
                Log.Error(e, "{Name} error!", request.GetType().Name);
                return Result.Failure<long>(e.Message);
            }
        }
    }
}