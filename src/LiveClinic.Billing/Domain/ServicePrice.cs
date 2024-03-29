﻿using System;
using CSharpFunctionalExtensions;
using LiveClinic.Shared.Domain;

namespace LiveClinic.Billing.Domain
{
    public class ServicePrice : Entity<long>
    {
        public Service Service { get; private set;}
        public Money Price { get; private set;}
        public DateTime EffectiveDate { get; private set;}

        private ServicePrice()
        {
        }
    
        public ServicePrice(Service service, Money price,DateTime effectiveDate)
        {
            Id = DateTime.Now.Ticks;
            Service = service;
            Price = price;
            EffectiveDate = effectiveDate;
        }

        public override string ToString()
        {
            return $"{Service} @ {Price}";
        }
    }
}