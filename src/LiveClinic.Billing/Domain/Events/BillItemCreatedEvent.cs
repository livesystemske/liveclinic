using System;
using MediatR;

namespace LiveClinic.Billing.Domain.Events;

public class BillItemCreatedEvent:INotification
{
    public long Id { get;}
    public DateTime Occured { get; }=DateTime.Now;

    public BillItemCreatedEvent(long encounterId)
    {
        Id = encounterId;
    }
}