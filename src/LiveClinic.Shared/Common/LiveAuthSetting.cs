using System.Collections.Generic;

namespace LiveClinic.Shared.Common
{
    public class LiveAuthSetting
    {
        public const string Key = "LiveAuth";
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public string Scope { get; set; }
        public string Flow { get; set; }
        public Dictionary<string, string> Scopes => GetScopes();

        private Dictionary<string, string> GetScopes()
        {
            if (!string.IsNullOrWhiteSpace(Scope))
                return new() { { Scope, Scope } };

            return new();
        }
    }
    
    public class TransportSetting
    {
        public const string Key = "Transport";
        public string Mode { get; set; }
        public string Host { get; set; }
        public string VHost { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public TransportSetting()
        {
        }

        public TransportSetting(string mode, string host,string vhost, string user, string password)
        {
            Mode = mode;
            Host = host;
            VHost = vhost;
            User = user;
            Password = password;
        }
    }
}