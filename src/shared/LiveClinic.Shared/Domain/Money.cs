using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace LiveClinic.Shared.Domain
{
    public class Money:ValueObject
    {
        public Currency Currency { get; private set;}
        public Double Value { get; private set;}

        public Money(double value,Currency currency)
        {
            Value = value;
            Currency = currency;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Currency;
            yield return Value;
        }

        public override string ToString()
        {
            return $"{Value:N} {Currency}";
        }

        public static Money From(double val,Currency currency)
        {
            return new Money(val, currency);
        }
    }
}