namespace LiveClinic.Contracts
{
    public record PatientRegistration
    {
        public long PatientId { get; set; }
        public string PatientName { get; set; }
        public long EncounterId { get; set; }
    }
}