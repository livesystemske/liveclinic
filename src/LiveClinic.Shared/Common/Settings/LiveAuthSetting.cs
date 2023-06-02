using System.Collections.Generic;
using System.Linq;

namespace LiveClinic.Shared.Common.Settings
{
    public class LiveAuthSetting
    {
        public const string Key = "LiveAuth";
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public string Scope { get; set; }
        public string Flow { get; set; }
        public string Mode  { get; set; }
        public Dictionary<string, string> Scopes => GetScopes();
        public string[] ReadScopes => Scopes.Select(x => x.Key).ToArray();

        public LiveAuthSetting()
        {
        }

        public LiveAuthSetting(string authority, string clientId, string secret, string scope,string flow)
        {
            Authority = authority;
            ClientId = clientId;
            Secret = secret;
            Scope = scope;
            Flow = flow;
        }

        private Dictionary<string, string> GetScopes()
        {
            var dict = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(Scope))
                foreach (var s in Scope.Split(','))
                    dict.Add($"{s}",$"{s}");

            return dict;
        }
    }
}
