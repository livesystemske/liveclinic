using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Registry.Application.Dtos;
using LiveClinic.Registry.Domain;
using LiveClinic.Registry.Domain.Events;
using LiveClinic.Registry.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LiveClinic.Registry.Application.Commands
{
    public class CreateEncounterCommand : IRequest<Result>
    {
        public NewEncounterDto NewEncounter { get; }

        public CreateEncounterCommand(NewEncounterDto newEncounter)
        {
            NewEncounter = newEncounter;
        }
    }

    public class CreateEncounterCommandHandler : IRequestHandler<CreateEncounterCommand, Result>
    {
        private readonly IMediator _mediator;
        private readonly RegistryDbContext _context;

        public CreateEncounterCommandHandler(IMediator mediator, RegistryDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task<Result> Handle(CreateEncounterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var patient = _context
                    .Patients.AsNoTracking()
                    .FirstOrDefault(x => x.Id == request.NewEncounter.PatientId);
                
                if (null == patient)
                    throw new ArgumentException("Patient not found!");
                
                var encounter = Encounter.From(request.NewEncounter);
                await _context.AddAsync(encounter,cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await _mediator.Publish(new EncounterCreatedEvent(
                        encounter.Id, $"{patient.PatientName}", encounter.Id, encounter.Service),
                    cancellationToken);
                
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