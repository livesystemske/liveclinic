using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Registry.Application.Dtos;
using LiveClinic.Registry.Domain;
using LiveClinic.Registry.Domain.Events;
using LiveClinic.Registry.Infrastructure.Data;
using MediatR;
using Serilog;

namespace LiveClinic.Registry.Application.Commands
{
    public class RegisterPatientCommand : IRequest<Result>
    {
        public NewPatientDto NewPatient { get; }

        public RegisterPatientCommand(NewPatientDto newPatient)
        {
            NewPatient = newPatient;
        }
    }

    public class RegisterPatientCommandHandler : IRequestHandler<RegisterPatientCommand, Result>
    {
        private readonly IMediator _mediator;
        private readonly RegistryDbContext _context;

        public RegisterPatientCommandHandler(IMediator mediator, RegistryDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task<Result> Handle(RegisterPatientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var patient = Patient.From(request.NewPatient);
                await _context.AddAsync(patient, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
                await _mediator.Publish(new PatientRegisteredEvent(patient.Id,$"{patient.PatientName}",patient.RegistrationEncounter), cancellationToken);
                
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