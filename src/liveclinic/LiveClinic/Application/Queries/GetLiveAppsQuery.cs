using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Domain;
using LiveClinic.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LiveClinic.Application.Queries
{
    public class GetLiveAppsQuery:IRequest<Result<List<LiveApp>>>
    {
    }

    public class GetLiveAppsQueryHandler:IRequestHandler<GetLiveAppsQuery,Result<List<LiveApp>>>
    {
        private readonly LiveClinicDbContext _context;

        public GetLiveAppsQueryHandler(LiveClinicDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<LiveApp>>> Handle(GetLiveAppsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var list = await _context.Apps.AsNoTracking().ToListAsync(cancellationToken);
                return Result.Success(list);
            }
            catch (Exception e)
            {
                Log.Error(e, "{Name} error!", request.GetType().Name);
                return Result.Failure<List<LiveApp>>(e.Message);
            }
        }
    }
}