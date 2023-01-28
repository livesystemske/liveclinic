using System;
using CSharpFunctionalExtensions;
using LiveClinic.Billing.Application.Dtos;
using LiveClinic.Shared.Domain;

namespace LiveClinic.Billing.Domain
{
    public class BillItem:Entity<long>
    {
        public long BillId { get; private set;}
        public long EncounterId { get; private set;}
        public Service Service { get; private set;}
        public Money Charge { get; private set;}
        public DateTime Created { get; private set;}

        private BillItem()
        {
        
        }

        public BillItem(long billId, long encounterId, Service service, Money charge)
        {
            Created=DateTime.Now;
            Id = Created.Ticks;
            BillId = billId;
            EncounterId = encounterId;
            Service = service;
            Charge = charge;
        }

        public static BillItem From(NewBillItemDto dto,Money charge)
        {
            return new BillItem(dto.BillId, dto.EncounterId, dto.Service, charge);
        }
    }
}