using System;
using LiveClinic.Shared.Domain;
using MediatR;

namespace LiveClinic.Registry.Domain.Events
{
    public class EncounterCreatedEvent:INotification
    {
        public long PatientId { get;  set;}
        public string PatientName { get;  set;}
        public long EncounterId { get;  set;}
        public Service Service { get;  set;}
        public DateTime Occured { get; }=DateTime.Now;

        public EncounterCreatedEvent(long patientId, string patientName, long encounterId, Service service)
        {
            PatientId = patientId;
            PatientName = patientName;
            EncounterId = encounterId;
            Service = service;
        }
    }
}