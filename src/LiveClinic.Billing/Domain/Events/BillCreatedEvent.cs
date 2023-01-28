using System;
using MediatR;

namespace LiveClinic.Billing.Domain.Events
{
    public class BillCreatedEvent:INotification
    {
        public long Id { get;}
        public DateTime Occured { get; }=DateTime.Now;

        public BillCreatedEvent(long encounterId)
        {
            Id = encounterId;
        }
    }
}