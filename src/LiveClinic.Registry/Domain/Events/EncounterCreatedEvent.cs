using System;
using MediatR;

namespace LiveClinic.Registry.Domain.Events
{
    public class EncounterCreatedEvent:INotification
    {
        public long Id { get;}
        public DateTime Occured { get; }=DateTime.Now;

        public EncounterCreatedEvent(long encounterId)
        {
            Id = encounterId;
        }
    }
}