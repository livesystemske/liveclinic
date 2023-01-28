using System;
using CSharpFunctionalExtensions;
using LiveClinic.Billing.Application.Dtos;
using LiveClinic.Shared.Domain;

namespace LiveClinic.Billing.Domain
{
    public class Payment:Entity<long>
    {
        public long BillId { get; private set;}
        public Money Amount { get; private set;}
        public DateTime Created { get; private set;}

        private Payment()
        {
        }

        public Payment(long billId, Money amount)
        {
            Created=DateTime.Now;
            Id = Created.Ticks;
            BillId = billId;
            Amount = amount;
        }

        public static Payment From(NewPaymentDto dto)
        {
            return new Payment(dto.BillId, Money.From(dto.Amount,dto.Currency));
        }
    }
}