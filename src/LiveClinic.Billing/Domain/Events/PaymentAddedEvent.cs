using System;
using MediatR;

namespace LiveClinic.Billing.Domain.Events;

public class PaymentAddedEvent:INotification
{
    public long Id { get;}
    public DateTime Occured { get; }=DateTime.Now;

    public PaymentAddedEvent(long encounterId)
    {
        Id = encounterId;
    }
}