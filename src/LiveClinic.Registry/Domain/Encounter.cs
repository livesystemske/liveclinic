using System;
using CSharpFunctionalExtensions;
using LiveClinic.Registry.Application.Dtos;
using LiveClinic.Shared.Common;
using LiveClinic.Shared.Domain;

namespace LiveClinic.Registry.Domain
{
    public class Encounter:Entity<long>
    {
        public long PatientId { get; private set;}
        public Service Service { get; private set;}
        public DateTime Created { get; private set; }

        private Encounter()
        {
        }

        public Encounter(long patientId, Service service, long? id = null)
        {
            Created = DateTime.Now;
            Id = id ?? LiveUtils.GenerateNewId();
            PatientId = patientId;
            Service = service;
        }

        public static Encounter From(NewEncounterDto dto)
        {
            return new Encounter(dto.PatientId, dto.Service);
        }
    }
}