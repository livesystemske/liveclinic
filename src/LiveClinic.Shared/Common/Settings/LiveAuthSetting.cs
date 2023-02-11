﻿using System.Collections.Generic;

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
        public Dictionary<string, string> Scopes => GetScopes();

        public LiveAuthSetting()
        {
        }

        public LiveAuthSetting(string authority, string clientId, string secret, string scope)
        {
            Authority = authority;
            ClientId = clientId;
            Secret = secret;
            Scope = scope;
        }

        private Dictionary<string, string> GetScopes()
        {
            if (!string.IsNullOrWhiteSpace(Scope))
                return new() { { Scope, Scope } };

            return new();
        }
        
    }
}
