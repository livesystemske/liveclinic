using System;
using CSharpFunctionalExtensions;

namespace LiveClinic.Domain
{
    public class UserSession :Entity<long>
    {
        public string ApplicationName  { get; set; } 
        public string SubjectId { get; set; }
        public string SessionId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Renewed { get; set; }
        public DateTime? Expires { get; set; }
        public string Ticket { get; set; } = default!;
        public string Key { get; set; } = default!;
    }
}