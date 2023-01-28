using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Registry.Data;
using LiveClinic.Registry.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LiveClinic.Registry.Application.Queries
{
    public class GetPatientQuery:IRequest<Result<Patient>>
    {
        public long Id { get;  }

        public GetPatientQuery(long id)
        {
            Id = id;
        }
    }

    public class GetPatientQueryHandler:IRequestHandler<GetPatientQuery,Result<Patient>>
    {
        private readonly RegistryDbContext _context;

        public GetPatientQueryHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Patient>> Handle(GetPatientQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var patient =await _context
                    .Patients.AsNoTracking()
                    .Include(x => x.Encounters)
                    .FirstOrDefaultAsync(f=>f.Id==request.Id,cancellationToken);

                return Result.Success(patient);
            }
            catch (Exception e)
            {
                Log.Error(e, "{Name} error!", request.GetType().Name);
                return Result.Failure<Patient>(e.Message);
            }
        }
    }
}