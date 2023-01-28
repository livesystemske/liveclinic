using System;
using System.Collections.Generic;
using System.Linq;
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
    public class GetPatientBillQuery : IRequest<Result<List<Bill>>>
    {
        public long PatientId { get; }

        public GetPatientBillQuery(long patientId)
        {
            PatientId = patientId;
        }
    }

    public class GetPatientBillQueryHandler : IRequestHandler<GetPatientBillQuery, Result<List<Bill>>>
    {
        private readonly BillingDbContext _context;

        public GetPatientBillQueryHandler(BillingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<Bill>>> Handle(GetPatientBillQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var list = await _context
                    .Bills.AsNoTracking()
                    .Include(x => x.Items)
                    .Include(i => i.Payments)
                    .Where(p => p.PatientId == request.PatientId)
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