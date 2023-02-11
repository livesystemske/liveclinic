using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Registry.Domain;
using LiveClinic.Registry.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LiveClinic.Registry.Application.Queries
{
    public class GetPatientsQuery:IRequest<Result<List<Patient>>>
    {
    }

    public class GetPatientsQueryHandler:IRequestHandler<GetPatientsQuery,Result<List<Patient>>>
    {
        private readonly RegistryDbContext _context;

        public GetPatientsQueryHandler(RegistryDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<Patient>>> Handle(GetPatientsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var list =await _context
                    .Patients.AsNoTracking()
                    .Include(x => x.Encounters)
                    .ToListAsync(cancellationToken);

                return Result.Success(list);
            }
            catch (Exception e)
            {
                Log.Error(e, "{Name} error!", request.GetType().Name);
                return Result.Failure<List<Patient>>(e.Message);
            }
        }
    }
}