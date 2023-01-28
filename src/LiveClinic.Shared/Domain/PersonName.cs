using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace LiveClinic.Shared.Domain
{
    public class PersonName:ValueObject
    {
        public string FirstName { get; private set;}
        public string LastName { get; private set;}

        public PersonName()
        {
        }

        public PersonName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName.ToUpper();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
        }

        public override string ToString()
        {
            return $"{LastName}, {FirstName}";
        }

        public static PersonName From(string firstName, string lastName)
        {
            return new PersonName(firstName, lastName);
        }
    }
}