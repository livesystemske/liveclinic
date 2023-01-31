using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using LiveClinic.Billing.Application.Dtos;
using LiveClinic.Shared.Common;

namespace LiveClinic.Billing.Domain
{
    public class Bill:Entity<long>
    {
        private readonly List<BillItem> _items=new List<BillItem>();
        private readonly List<Payment> _payments=new List<Payment>();
    
        public string Invoice { get; private set;}
        public long PatientId { get; private set;}
        public string PatientName { get; private set;}
        public DateTime Created { get; private set;}
        public IReadOnlyCollection<BillItem> Items => _items;
        public IReadOnlyCollection<Payment> Payments => _payments;
        public bool IsAlreadyPaid => CheckPaid();

        private Bill()
        {
        }

        public Bill(long patientId, string patientName,long? id=null)
        {
            Created = DateTime.Now;
            Id = id ?? LiveUtils.GenerateNewId();
            
            Invoice = Id.GenerateInvoice();
            PatientId = patientId;
            PatientName = patientName;
        }

        public static Bill From(NewBillDto dto)
        {
            return new Bill(dto.PatientId, dto.PatientName);
        }

        private bool CheckPaid()
        {
            var total = Items.Sum(x => x.Charge.Value);
            var paid = Payments.Sum(x => x.Amount.Value);

            return paid >= total;
        }
    }
}