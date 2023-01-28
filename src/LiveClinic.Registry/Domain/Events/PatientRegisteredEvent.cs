using System;
using MediatR;

namespace LiveClinic.Registry.Domain.Events
{
    public class PatientRegisteredEvent:INotification
    {
        public long Id { get;}
        public DateTime Occured { get; }=DateTime.Now;

        public PatientRegisteredEvent(long encounterId)
        {
            Id = encounterId;
        }
    }
}