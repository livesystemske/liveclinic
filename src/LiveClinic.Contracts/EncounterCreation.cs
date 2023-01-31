namespace LiveClinic.Contracts;

public record EncounterCreation
{
    public long PatientId { get;  set;}
    public string PatientName { get;  set;}
    public long EncounterId { get;  set;}
    public int Service { get;  set;}
}