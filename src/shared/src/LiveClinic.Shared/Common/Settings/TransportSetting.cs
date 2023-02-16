namespace LiveClinic.Shared.Common.Settings
{
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