using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Registry.Application.Dtos;
using LiveClinic.Registry.Data;
using LiveClinic.Registry.Domain;
using LiveClinic.Registry.Domain.Events;
using MediatR;
using Serilog;

namespace LiveClinic.Registry.Application.Commands
{
    public class UpdatePatientCommand : IRequest<Result>
    {
        public EditPatientDto Patient { get; }

        public UpdatePatientCommand(EditPatientDto patient)
        {
            Patient = patient;
        }
    }

    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, Result>
    {
        private readonly IMediator _mediator;
        private readonly RegistryDbContext _context;

        public UpdatePatientCommandHandler(IMediator mediator, RegistryDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task<Result> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(request.Patient.Id);

                if (null == patient)
                    throw new Exception("Not found");

                patient.UpdateFrom(request.Patient);
                
                _context.Update(patient);
                await _context.SaveChangesAsync(cancellationToken);
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
