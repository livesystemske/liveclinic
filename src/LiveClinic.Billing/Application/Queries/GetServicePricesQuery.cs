using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Billing.Data;
using LiveClinic.Billing.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LiveClinic.Billing.Application.Queries
{
    public class GetServicePricesQuery:IRequest<Result<List<ServicePrice>>>
    {
    }

    public class GetServicePricesQueryHandler:IRequestHandler<GetServicePricesQuery,Result<List<ServicePrice>>>
    {
        private readonly BillingDbContext _context;

        public GetServicePricesQueryHandler(BillingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<ServicePrice>>> Handle(GetServicePricesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var list =await _context
                    .ServicePrices.AsNoTracking()
                    .ToListAsync(cancellationToken);

                return Result.Success(list);
            }
            catch (Exception e)
            {
                Log.Error(e, "{Name} error!", request.GetType().Name);
                return Result.Failure<List<ServicePrice>>(e.Message);
            }
        }
    }
}