using System;
using LiveClinic.Shared.Domain;
using MediatR;

namespace LiveClinic.Billing.Domain.Events
{
    public class BillCreatedEvent:INotification
    {
        public long BillId { get;  set;}
        public long EncounterId { get;  set;}
        public Service Service { get;  set;}
        public DateTime Occured { get; }=DateTime.Now;

        public BillCreatedEvent(long billId, long encounterId, Service service)
        {
            BillId = billId;
            EncounterId = encounterId;
            Service = service;
        }
    }
}