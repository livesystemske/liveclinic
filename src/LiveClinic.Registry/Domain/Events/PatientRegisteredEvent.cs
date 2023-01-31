using System;
using MediatR;

namespace LiveClinic.Registry.Domain.Events
{
    public class PatientRegisteredEvent:INotification
    {
        public long PatientId { get;}
        public string PatientName { get;}
        public long EncounterId { get;}
        
        public DateTime Occured { get; }=DateTime.Now;

        public PatientRegisteredEvent(long patientId, string patientName, long encounterId)
        {
            PatientId = patientId;
            PatientName = patientName;
            EncounterId = encounterId;
        }
    }
}