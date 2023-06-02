using LiveClinic.Shared.Domain;

namespace LiveClinic.Registry.Application.Dtos
{
    public class NewEncounterDto
    {
        public long PatientId { get;  set;}
        public Service Service { get;  set;}
    }
}