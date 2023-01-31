using System;
using LiveClinic.Shared.Domain;

namespace LiveClinic.Registry.Application.Dtos
{
    public class NewPatientDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get;  set;}
    }
}