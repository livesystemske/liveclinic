using CSharpFunctionalExtensions;

namespace LiveClinic.Domain
{
    public class LiveApp : Entity<string>
    {
        public string Name { get; set; }

        public LiveApp(string name)
        {
            Id = $"LiveClinic.{name}";
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}