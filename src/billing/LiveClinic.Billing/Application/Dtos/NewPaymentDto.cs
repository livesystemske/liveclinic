using System;
using LiveClinic.Shared.Domain;

namespace LiveClinic.Billing.Application.Dtos
{
    public class NewPaymentDto
    {
        public long BillId { get;  set;}
        
        public Currency Currency { get;  set;}
        public Double Amount { get;  set;}
    }
}