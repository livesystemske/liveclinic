using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Billing.Domain;
using LiveClinic.Billing.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LiveClinic.Billing.Application.Queries
{
    public class GetBillsQuery:IRequest<Result<List<Bill>>>
    {
    }

    public class GetBillsQueryHandler:IRequestHandler<GetBillsQuery,Result<List<Bill>>>
    {
        private readonly BillingDbContext _context;

        public GetBillsQueryHandler(BillingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<Bill>>> Handle(GetBillsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var list =await _context
                    .Bills.AsNoTracking()
                    .Include(x=>x.Items)
                    .Include(i=>i.Payments)
                    .ToListAsync(cancellationToken);

                return Result.Success(list);
            }
            catch (Exception e)
            {
                Log.Error(e, "{Name} error!", request.GetType().Name);
                return Result.Failure<List<Bill>>(e.Message);
            }
        }
    }
}