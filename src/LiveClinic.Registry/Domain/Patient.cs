using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using LiveClinic.Registry.Application.Dtos;
using LiveClinic.Shared.Common;
using LiveClinic.Shared.Domain;

namespace LiveClinic.Registry.Domain
{
    public class Patient:Entity<long>
    {
        private readonly List<Encounter> _encounters=new List<Encounter>();
        
        public string MemberNo { get; private set; }
        public PersonName PatientName { get; private set; } = new PersonName();
        public Gender Gender { get; private set; }
        public DateTime BirthDate { get;  private set;}
        public DateTime Created { get;  private set;}
        public IReadOnlyCollection<Encounter> Encounters => _encounters;
        public long RegistrationEncounter => _encounters.First(x => x.Service == Service.Registration).Id;

        private Patient()
        {
        }
        
        public Patient(PersonName patientName, Gender gender, DateTime birthDate,long? id=null)
        {
            Id = id ?? LiveUtils.GenerateId();
            MemberNo = Id.GenerateMemberNo();
            PatientName = patientName;
            Gender = gender;
            BirthDate = birthDate;
            Created = DateTime.Now;
            Enroll();
        }

        private void Enroll()
        {
            var encounter = new Encounter(Id, Service.Registration);
            _encounters.Add(encounter);
        }
        
        public static Patient From(NewPatientDto dto)
        {
            return new Patient(
                PersonName.From(dto.FirstName,dto.LastName),
                dto.Gender,
                dto.BirthDate
            );
        }

        public override string ToString()
        {
            return $"{MemberNo} | {PatientName} ({Gender}),{BirthDate:yyyy MMM dd} ";
        }
    }
}